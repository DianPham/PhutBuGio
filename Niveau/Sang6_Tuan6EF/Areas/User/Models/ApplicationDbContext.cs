using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models;
using Niveau.Areas.Admin.Models.Employees;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.User.Models.ShoppingCart;

namespace Niveau.Areas.User.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }
        public DbSet<Employee> Employees { get; set; }

        //Product
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        //Account
        public DbSet<ApplicationUser> Accounts { get; set; }
    }
}
