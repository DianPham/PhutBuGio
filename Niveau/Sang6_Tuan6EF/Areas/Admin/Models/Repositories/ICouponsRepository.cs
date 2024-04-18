using Niveau.Areas.Admin.Models.Products;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public interface ICouponsRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> GetByIdAsync(int id);
        Task AddAsync(Coupon category);
        Task UpdateAsync(Coupon category);
        Task DeleteAsync(int id);
        Task<CouponVerificationResult> VerifyCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetAllActiveAsync();
    }
}
