using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.Admin.Models.Repositories;
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _couponRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
