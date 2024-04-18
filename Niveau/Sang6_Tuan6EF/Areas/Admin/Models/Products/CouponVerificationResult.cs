using static Niveau.Areas.Admin.Models.Products.Coupon;

namespace Niveau.Areas.Admin.Models.Products
{
    public class CouponVerificationResult
    {
        public bool IsValid { get; set; }
        public int CouponId { get; set; }      
        public string Message { get; set; }
        public double DiscountAmount { get; set; }
        public DiscountType Type { get; set; }
        public double? MinimumSpend { get; set; }
    }
}
