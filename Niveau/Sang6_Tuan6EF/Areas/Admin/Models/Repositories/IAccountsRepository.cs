using Niveau.Areas.Admin.Models.Accounts;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface IAccountsRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task AddAsync(ApplicationUser account);
        Task UpdateAsync(ApplicationUser account);
        Task DeleteAsync(string id);
    }

}
