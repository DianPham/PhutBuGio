﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Niveau.Areas.Admin.Models;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.Admin.Models.Repositories;

namespace Niveau.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly IProductsRepository _productRepository;
        private readonly ICategoriesRepository _categoryRepository;
        public ProductsController(IProductsRepository productRepository,
        ICategoriesRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        // Hiển thị danh sách sản phẩm
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> List()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        // Hiển thị form thêm sản phẩm mới
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        // Xử lý thêm sản phẩm mới
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                if (imageUrls != null)
                {
                    product.Images = new List<ProductImage>();
                    foreach (var file in imageUrls)
                    {
                        ProductImage image = new ProductImage();
                        image.Product = product;
                        image.ProductId = product.Images.Max(i => i.ProductId) + 1;
                        // Lưu các hình ảnh khác
                        product.Images.Add(await SaveImage(file, image));
                    }
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction("List");
            }
            return View(product);
        }
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        private async Task<ProductImage> SaveImage(IFormFile image, ProductImage pImage)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            pImage.Url = "/images/" + image.FileName;

            return pImage; // Trả về đường dẫn tương đối
        }
        //Nhớ tạo folder images trong wwwroot

        // Hiển thị thông tin chi tiết sản phẩm
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Hiển thị form cập nhật sản phẩm
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name",
            product.CategoryId);
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product,
        IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await
                _productRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
                                                     // Giữ nguyên thông tin hình ảnh nếu không có hình mới được

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới

                    product.ImageUrl = await SaveImage(imageUrl);

                }
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(List));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
