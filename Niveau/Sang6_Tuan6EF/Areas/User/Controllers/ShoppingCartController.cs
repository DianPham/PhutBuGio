using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.Admin.Models.Repositories;
using Niveau.Areas.User.Models.ShoppingCart;
using Niveau.Areas.User.Models;
using Niveau.Areas.User.Extentions;
using Niveau.Areas.Admin.Models.Accounts;
using Newtonsoft.Json.Linq;
using Niveau.Others;
using NiveauOther;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Niveau.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductsRepository _productRepository;
        private readonly ApplicationDbContext _context;
        //quản lý đăng nhập người dùng
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(IProductsRepository productRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl,

            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok();
        }

        //tìm sản phẩm trong DB dựa vào productID
        private async Task<Product> GetProductFromDatabase(int productID)
        {
            var product = await _productRepository.GetByIdAsync(productID);
            return product;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            /*if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index", "Home");
            }*/
            return View(cart);
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        //Checkout: dùng để người dùng nhập thông tin về địa chỉ giao hàng/ ghi chú khi đặt hàng
        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new Order());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            /*await Payment(order.TotalPrice);*/

            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
        }
        public static string codeThanhToanTransferFromMoMo;
        public static string soTienTransferFromMoMo;
        public static string idTourTransferFromMoMo;
        public static string payurlTEst;
        public async Task<IActionResult> InitiatePayment(decimal totalAmount)
        {
            // Tạo request và chuyển hướng đến MoMo
            // Đoạn code tạo request và chuyển hướng đã được giải thích ở câu trả lời trước
            // Bạn có thể sao chép và paste đoạn code từ câu trước vào đây
            // Ví dụ:
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán phí mua hàng.";
            string returnUrl = "https://localhost:7030/ShoppingCart/OrderCompleted";
            string notifyUrl = "https://yourdomain.com/Home/SavePayment";

            string amount = totalAmount.ToString();
            string orderId = DateTime.Now.Ticks.ToString(); // Mã đơn hàng duy nhất
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            // Tạo chữ ký
            string rawSignature = "partnerCode=" + partnerCode + "&accessKey=" + accessKey +
                "&requestId=" + requestId + "&amount=" + amount +
                "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&returnUrl=" + returnUrl +
                "&notifyUrl=" + notifyUrl + "&extraData=" + extraData;
            string signature = CalculateHMACSHA256Hash(rawSignature, serectkey);

            // Tạo URL redirect đến MoMo
            string redirectUrl = endpoint + "?partnerCode=" + partnerCode +
                "&accessKey=" + accessKey + "&requestId=" + requestId +
                "&amount=" + amount + "&orderId=" + orderId +
                "&orderInfo=" + orderInfo + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl) +
                "&notifyUrl=" + HttpUtility.UrlEncode(notifyUrl) +
                "&extraData=" + HttpUtility.UrlEncode(extraData) + "&signature=" + signature;

            // Chuyển hướng đến trang thanh toán MoMo
            return Redirect(redirectUrl);
        }

        private string CalculateHMACSHA256Hash(string input, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashValue);
            }
        }

        public async Task<IActionResult> Payment(decimal totalMOMO)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán online";
            string returnUrl = "https://localhost:7030/ShoppingCart/OrderCompleted";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "10000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            // truyen du lieu sang paymentconfirm
            soTienTransferFromMoMo = amount.ToString();
            codeThanhToanTransferFromMoMo = orderid.ToString();
            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
     {
         { "partnerCode", partnerCode },
         { "accessKey", accessKey },
         { "requestId", requestId },
         { "amount",amount},
         { "orderId", orderid },
         { "orderInfo", orderInfo },
         { "returnUrl", returnUrl },
         { "notifyUrl", notifyurl },
         { "extraData", extraData },
         { "requestType", "captureMoMoWallet" },
         { "signature", signature }
     };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            payurlTEst = jmessage.ToString();
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public IActionResult OrderCompleted()
        {
            return View();
        }
    }
}
