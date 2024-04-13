using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.User.Models;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public class EFCouponsRepository : ICouponsRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCouponsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //các lệnh thực thi Interface
        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.ToListAsync();
        }
        public async Task<Coupon> GetByIdAsync(int id)
        {
            return await _context.Coupons.FindAsync(id);
        }
        public async Task AddAsync(Coupon Coupon)
        {
            _context.Coupons.Add(Coupon);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Coupon Coupon)
        {
            _context.Coupons.Update(Coupon);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var Coupon = await _context.Coupons.FindAsync(id);
            _context.Coupons.Remove(Coupon);
            await _context.SaveChangesAsync();
        }
    }
}
