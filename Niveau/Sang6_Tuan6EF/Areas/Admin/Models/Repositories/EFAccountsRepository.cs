using Microsoft.EntityFrameworkCore;
using Niveau.Models;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public class EFAccountsRepository : IAccountsRepository
    {
        private readonly ApplicationDbContext _context;

        public EFAccountsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(ApplicationUser account)
        {
            _context.Users.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Users.FindAsync(id);
            if (account != null)
            {
                _context.Users.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }

}
