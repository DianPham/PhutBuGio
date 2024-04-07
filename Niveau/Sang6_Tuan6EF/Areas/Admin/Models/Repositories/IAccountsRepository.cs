namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface IAccountsRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(int id);
        Task AddAsync(ApplicationUser account);
        Task UpdateAsync(ApplicationUser account);
        Task DeleteAsync(int id);
    }

}
