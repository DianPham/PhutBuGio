using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Niveau.Areas.User.Models.ShoppingCart;
using Niveau.Areas.Admin.Models.Repositories;
using System.Collections.Generic;
using static Niveau.Areas.User.Models.ShoppingCart.Order;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Niveau.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _orderRepository;
        public OrdersController(IOrdersRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Index()
        {
            var order = await _orderRepository.GetAllAsync();
            return View(order);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _orderRepository.UpdateAsync(order);
                return RedirectToAction(nameof(Index));

            }
            return View(order);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
