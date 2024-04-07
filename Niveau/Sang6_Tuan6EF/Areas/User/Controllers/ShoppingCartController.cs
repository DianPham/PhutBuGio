using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Niveau.Extentions;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.Admin.Models.Repositories;
using Niveau.Models;
using Niveau.Areas.Admin.Models;
using Niveau.Models.ShoppingCart.ShoppingCart;
using NiveauOther;
using Newtonsoft.Json.Linq;
using Niveau.Others;

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
            var product = await GetProductFromDatabase(productId);
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
            return RedirectToAction("Index", "Home");
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
        //khi người dùng nhấn nút đặt hàng thì lưu thông tin vào DB
        public async Task<IActionResult> Checkout(Order order)
        {

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(User);//lấy thông tin người dùng đã đăng nhập
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

            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
            // lúc này nếu thanh toán thất bại thì phương thức thanh toán sẽ auto là tiền mặt khi nhận hàng.
        }
        public IActionResult OrderCompleted()
        {
            return View();
        }

        public ActionResult Payment(int tongTien)
        {
            //thông số yêu cầu cần gửi tới hệ thống MoMo
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán online";
            string returnUrl = "https://localhost:7030";
            string notifyurl = "https://Home/SavePayment"; // Địa chỉ URL mà MoMo sẽ gửi kết quả thanh toán đến. 
            string amount = tongTien.ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

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
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount",amount },
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
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

    }
}
