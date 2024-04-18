using System.ComponentModel.DataAnnotations;
using static Niveau.Areas.Admin.Models.Products.Coupon;

namespace Niveau.Areas.Admin.Models.Products
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // Unique code of the coupon

        public enum DiscountType
        {
            Percentage,
            FixedAmount
        }

        [Required]
        public DiscountType Type { get; set; } // Type of discount (percentage or fixed amount)

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Discount must be greater than 0")]
        public double Discount { get; set; } // Discount amount or percentage

        [Required]
        public DateTime ExpiryDate { get; set; } // Expiration date of the coupon

        [Range(0, Double.MaxValue, ErrorMessage = "Minimum spend must be non-ne4gative")]
        public double? MinimumSpend { get; set; } // Optional minimum spend to apply the coupon

        public bool IsActive { get; set; } // Indicates if the coupon is active

        // Optional: Restrict coupon to specific categories or products
        public string? ApplicableToCategory { get; set; } // Nullable if not applicable

        // Optional: Limit the number of times a coupon can be used
        public int? MaxUses { get; set; }
    }
    
}
