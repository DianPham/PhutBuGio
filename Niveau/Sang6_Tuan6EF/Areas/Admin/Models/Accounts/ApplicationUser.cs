using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Niveau.Areas.Admin.Models.Accounts
{
    public class ApplicationUser : IdentityUser
    {
        //ApplicationUser= IdentityUser + thuộc tính riêng
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Status {  get; set; } = true;
        public string? Message { get; set; }

    }
}
