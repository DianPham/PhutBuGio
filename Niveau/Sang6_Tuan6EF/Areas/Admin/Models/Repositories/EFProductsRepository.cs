using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.User.Models;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public class EFProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //các lệnh thực thi Interface
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                             .Include(p => p.Images) // Assuming Product has a navigation property called ProductImages
                             .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _context.Products
                             .Include(p => p.Images)
                             .Where(p => p.IsActive)// Assuming Product has a navigation property called ProductImages
                             .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                         .Include(p => p.Images) // Eager load the product images
                         .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            // Load the existing product with its images
            var existingProduct = await _context.Products
                                        .Include(p => p.Images)
                                        .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            // Update the scalar properties of the product
            _context.Entry(existingProduct).CurrentValues.SetValues(product);

            // Handle the ProductImages collection
            foreach (var existingImage in existingProduct.Images.ToList())
            {
                if (!product.Images.Any(pi => pi.Id == existingImage.Id))
                {
                    // If an existing image is not in the updated product's images, remove it
                    _context.ProductImages.Remove(existingImage);
                }
            }

            foreach (var updatedImage in product.Images)
            {
                var existingImage = existingProduct.Images
                                                   .FirstOrDefault(pi => pi.Id == updatedImage.Id);

                if (existingImage != null)
                {
                    // Update existing image
                    _context.Entry(existingImage).CurrentValues.SetValues(updatedImage);
                }
                else
                {
                    // Add new image (set foreign key if necessary)
                    updatedImage.ProductId = existingProduct.Id;
                    existingProduct.Images.Add(updatedImage);
                }
            }

            // Save changes
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryId(int subcategoryId)
        {
            return await _context.Products
                  .Where(p => p.CategoryId == subcategoryId)
                  .ToListAsync();
        }
        public async Task<IEnumerable<string>> SearchAsync(string term)
        {
            var products = await GetAllActiveAsync();
            var queryResult = products.
                                Where(p => p.Name.ToLower().StartsWith(term)) // Case insensitive search
                                .OrderBy(p => p.Name) // Order by name
                                .Select(p => p.Name)    // Select only the product name
                                .ToList();// Get the first result or null if no match
            return queryResult;
        }
    }
}
