using Microsoft.EntityFrameworkCore;
using Niveau.Areas.User.Models.ShoppingCart;
using Niveau.Areas.User.Models;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public class EFOrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        public EFOrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //các lệnh thực thi Interface
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
