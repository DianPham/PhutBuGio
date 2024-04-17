using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Niveau.Areas.Admin.Models;
using Niveau.Areas.Admin.Models.Accounts;
using Niveau.Areas.Admin.Models.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Niveau.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AccountsController : Controller
    {
        private readonly IAccountsRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(IAccountsRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            //var accounts = await _repository.GetAllAsync();

            var accountsViewModel = new List<AccountViewModel>();

            foreach (var user in _userManager.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                accountsViewModel.Add(new AccountViewModel { User = user, Roles = roles });
            }

            return View(accountsViewModel);
            //return View(accounts);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // GET: Account/Create
        

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser account) // Changed id to string
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Status(string id, bool state, string message)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                product.Status = state; // Assuming 'IsActive' is a property to toggle enable/disable
                product.Message = message;
                await _repository.UpdateAsync(product);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Product not found." });
        }
    }
}


