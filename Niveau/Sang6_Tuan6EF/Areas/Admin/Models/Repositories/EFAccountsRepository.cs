using Microsoft.EntityFrameworkCore;
using Niveau.Areas.User.Models;

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
            return await _context.Accounts.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task AddAsync(ApplicationUser account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }

}
