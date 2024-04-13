using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models.Accounts;
using Niveau.Areas.Admin.Models.Employees;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.User.Models.ShoppingCart;

namespace Niveau.Areas.User.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entity configuration for Product and its relationship to ProductImage
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Product) // or .WithOne(pi => pi.Product) if ProductImage has a navigation property back to Product
                .HasForeignKey(pi => pi.ProductId); // Assuming ProductImage has a ProductId foreign key

            //Seed Data
            string ADMIN_ID = "39c37cf7-806e-45e0-b42c-9c3dcdc8c990";
            string ROLE_ID = "08671a98-cc9b-4323-b23b-04c25925c2c0";
            string ACCOUNT_ID = "0e337acc-137a-49e7-b9fa-8e741e9792ed";

            //seed admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = ADMIN_ID,
                ConcurrencyStamp = ROLE_ID
            });

            //create user
            var appUser = new ApplicationUser
            {
                Id = ACCOUNT_ID,
                Email = "admin@gmail.com",
                EmailConfirmed = false,
                UserName = "admin@gmail.com",
                FirstName = "seed",
                LastName = "admin",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                LockoutEnabled = true,
            };

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "Admin123*");

            //seed user
            modelBuilder.Entity<ApplicationUser>().HasData(appUser);

            //set user role to admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ID,
                UserId = ACCOUNT_ID
            });
        }
        public DbSet<Employee> Employees { get; set; }

        //Product
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }       
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }


        //Account
        public DbSet<ApplicationUser> Accounts { get; set; }
    }
}
