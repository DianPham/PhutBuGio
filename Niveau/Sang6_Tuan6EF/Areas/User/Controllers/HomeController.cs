using Microsoft.AspNetCore.Mvc;
using Niveau.Areas.Admin.Models.Repositories;
using System.Diagnostics;
using Niveau.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace Niveau.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IProductsRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductsRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        [HttpGet("Home")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var product = await _productRepository.GetAllAsync();
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Contact()
        {
            return View();
        }
    }
}