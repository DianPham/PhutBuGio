﻿using Microsoft.AspNetCore.Authorization;
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
using Microsoft.Data.SqlClient;
using System.Data;

namespace Niveau.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        public static int _cartId = 0; // madonhang xu ly
        public static int _totalPrice = 0; // so tien duoc xu ly của _cartId
        public static ShoppingCart _cart = new ShoppingCart();
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
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart(Guid.NewGuid().ToString());
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { TotalItems = CalculateTotalItems() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart([FromBody] CartUpdateModel model)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart(Guid.NewGuid().ToString());

            foreach (var item in model.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity = item.Quantity;
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { success = true });
        }

        public class CartUpdateModel
        {
            public List<CartItemUpdate> Items { get; set; }
        }

        public class CartItemUpdate
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        //tìm sản phẩm trong DB dựa vào productID
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart(Guid.NewGuid().ToString());
            /*if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index", "Home");
            }*/
            return View(cart);
        }

        private int CalculateTotalItems()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items != null)
            {
                return cart.Items.Count();
            }
            return 0;
        }

        public IActionResult GetTotalItemsInCart()
        {
            int totalItems = CalculateTotalItems();
            return Ok(totalItems);
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

            _cart = cart;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            _cartId = order.Id;
            _totalPrice = Convert.ToInt32(order.TotalPrice);
            return RedirectToAction("OrderCompleted");

        }

        // Bắt đầu phần xử lý thanh toán MOMO
        private string CalculateHMACSHA256Hash(string input, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashValue);
            }
        }

        public async Task<IActionResult> Payment()
        {           
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            var totalAmount = cart.Items.Sum(i => i.Price * i.Quantity);

            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán online cho Đơn hàng ";
            string returnUrl = "https://localhost:7030/User/ShoppingCart/XulyKQMomo" ;
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = Convert.ToInt32(totalAmount).ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";



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


            return Redirect(jmessage.GetValue("payUrl").ToString());

        }
        //===================================================================


        public IActionResult OrderCompleted()
        {
            ViewBag.id = _cartId;
            ViewBag.Sotien = _totalPrice;
            ViewBag.Cart = _cart;
            return View();
        }
        public IActionResult NoneSuccses()
        {

            return View();
        }

        //==========================================
        public SqlConnection connection = new SqlConnection(@"Server=LAPTOP-VVVS6ML6;Database=Niveau;Trusted_Connection=True;TrustServerCertificate=True");
        public void CheckConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void SetTrangThaiDonHang(int id, int status)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("UPDATE Orders SET status = {0} WHERE Id = {1}", status, id);
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read() == true)
            {
                if (!reader.IsClosed)
                    reader.Close();
            }
            if (!reader.IsClosed)
                reader.Close();
            connection.Close();
        }


        public async Task<IActionResult> XulyKQMomo()
        {
            string q = HttpContext.Request.Query["errorCode"];
            var sotien = HttpContext.Request.Query["amount"];
            // var madonhang = HttpContext.Request.Query["MaDonHang"];

            // gán cho biến local truy cập Id và số tiền để thông báo ở view tiếp theo

            _totalPrice = Convert.ToInt32(sotien);
            //_cartId = Convert.ToInt32(madonhang);
            if (q.ToString() == "0")
            {
                //SetTrangThaiDonHang(Convert.ToInt32(madonhang), 1);
                return RedirectToAction("Checkout");// trả về trang checkout
            }
            else
            {
                //SetTrangThaiDonHang(Convert.ToInt32(madonhang), 0);
                return RedirectToAction("NoneSuccses");// trả về trang thánh toán thất bại
            }
        }

    }
}
