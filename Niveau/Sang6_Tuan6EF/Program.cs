using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models.Repositories;
using Niveau.Areas.User.Models;
using Niveau.Areas.Admin.Models.Accounts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductsRepository, EFProductsRepository>();
builder.Services.AddScoped<ICategoriesRepository, EFCategoriesRepository>();
builder.Services.AddScoped<IAccountsRepository, EFAccountsRepository>();
builder.Services.AddScoped<ICouponsRepository, EFCouponsRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.Scope.Add("profile");
    googleOptions.Scope.Add("email");
});

// Đặt trước AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Set here for Identity
    options.AccessDeniedPath = "/AccessDenied";
});
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
       name: "employee",
       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
   );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area:exists=User}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();
