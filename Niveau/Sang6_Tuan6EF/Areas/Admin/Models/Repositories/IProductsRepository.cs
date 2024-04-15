using System.Threading.Tasks;
using System.Collections.Generic;
using Niveau.Areas.Admin.Models.Products;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string searchString);
        Task<IEnumerable<string>> GetAllProductNames();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
