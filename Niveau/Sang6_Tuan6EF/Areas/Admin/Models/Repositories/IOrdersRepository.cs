using Niveau.Areas.User.Models.ShoppingCart;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
