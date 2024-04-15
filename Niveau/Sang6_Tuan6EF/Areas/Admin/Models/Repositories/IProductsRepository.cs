using Niveau.Areas.Admin.Models.Products;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllActiveAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<string>> SearchAsync(string term);

    }
}
