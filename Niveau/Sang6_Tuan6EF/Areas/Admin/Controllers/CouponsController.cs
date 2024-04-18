using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.Admin.Models.Repositories;
using Niveau.Areas.User.Extentions;
using System.Collections.Generic;
using static Niveau.Areas.Admin.Models.Products.Coupon;

namespace Niveau.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponsController : Controller
    {
        private readonly ICouponsRepository _couponRepository;
        public CouponsController(ICouponsRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Index()
        {
            var coupon = await _couponRepository.GetAllAsync();
            return View(coupon);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Details(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            ViewBag.DiscountTypes = Enum.GetValues(typeof(DiscountType))
                                .Cast<DiscountType>()
                                .Select(dt => new SelectListItem
                                {
                                    Text = dt.ToString(),
                                    Value = ((int)dt).ToString()
                                });
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                await _couponRepository.AddAsync(coupon);
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.DiscountTypes = Enum.GetValues(typeof(DiscountType))
                                .Cast<DiscountType>()
                                .Select(dt => new SelectListItem
                                {
                                    Text = dt.ToString(),
                                    Value = ((int)dt).ToString()
                                });
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id, Coupon coupon)
        {
            if (id != coupon.CouponId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _couponRepository.UpdateAsync(coupon);
                return RedirectToAction(nameof(Index));

            }
            return View(coupon);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyDiscountCode(string couponCode)
        {
            // Assume a service or method to check the discount and retrieve amount
            var result = await _couponRepository.VerifyCodeAsync(couponCode);
            if (result.IsValid)
            {
                return Json(new { success = true, id = result.CouponId, discount = result.DiscountAmount, type = result.Type.ToString(), minimum = result.MinimumSpend });
            }
            return Json(new { success = false, message = "Invalid or expired discount code." });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int CouponId)
        {
            await _couponRepository.DeleteAsync(CouponId);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Status(int id, bool state)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon != null)
            {
                coupon.IsActive = state; // Assuming 'IsActive' is a property to toggle enable/disable
                await _couponRepository.UpdateAsync(coupon);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Coupon not found." });
        }

        

        public IActionResult SetDiscountSession(int couponId)
        {
            HttpContext.Session.SetObjectAsJson("CouponId", couponId);

            return Ok();
        }
    }
}
