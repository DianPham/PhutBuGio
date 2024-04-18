using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using Niveau.Areas.Admin.Models.Accounts;

namespace Niveau.Areas.User.Models.ShoppingCart
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public int? CouponId { get; set; }
        public int? status { get; set; }
        public string? Notes { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
