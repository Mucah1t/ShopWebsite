using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppUI.EmailServices;
using ShopAppUI.Identity;
using System.Configuration.Provider;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager _configuration = builder.Configuration;
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationContext>(options =>
options.UseSqlServer("server=localhost\\SQLEXPRESS;database=DbShopApp;integrated security=true"));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;

    // Lockout                
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    // options.User.AllowedUserNameCharacters = "";
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true, // Denies access to cookies from javascript-style applications
        Name = ".ShopApp.Security.Cookie",
        SameSite = SameSiteMode.Strict
    };
});


builder.Services.AddScoped<IProductRepository, EfCoreProductRepository>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<ICategorryService, CategoryManager>();

builder.Services.AddScoped<ICartRepository, EfCoreCartRepository>();
builder.Services.AddScoped<ICartService, CartManager>();

builder.Services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderManager>();



builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:Password"]));
var app = builder.Build();


// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    SeedDatabase.Seed();
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();  // To activate Identity Library

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "orders",
    pattern: "orders",
    defaults: new { controller = "Cart", action = "GetOrders" }
              );

app.MapControllerRoute(
    name: "checkout",
    pattern: "checkout",
    defaults: new { controller = "Cart", action = "Checkout" }
              );

app.MapControllerRoute(
    name: "cart",
    pattern: "cart",
    defaults: new { controller = "Cart", action = "Index" }
              );

app.MapControllerRoute(
    name: "adminusers",
    pattern: "admin/user/list",
    defaults: new { controller = "Admin", action = "UserList" }
              );

app.MapControllerRoute(
    name: "adminuseredit",
    pattern: "admin/user/{id?}",
    defaults: new { controller = "Admin", action = "UserEdit" }
               );


app.MapControllerRoute(
    name: "adminroles",
    pattern: "admin/role/list",
    defaults: new { controller = "Admin", action = "RoleList" }
              );

app.MapControllerRoute(
    name: "adminrolecreate",
    pattern: "admin/role/create",
    defaults: new { controller = "Admin", action = "RoleCreate" }
               );

app.MapControllerRoute(
    name: "adminroleedit",
    pattern: "admin/role/{id?}",
    defaults: new { controller = "Admin", action = "RoleEdit" }
               );

app.MapControllerRoute(

    name: "adminproducts",
    pattern: "admin/products",
    defaults: new { controller = "Admin", action = "ProductList" }
    );
app.MapControllerRoute(
    name: "adminproductcreate",
    pattern: "admin/products/create",
    defaults: new { controller = "Admin", action = "ProductCreate" }
    );
app.MapControllerRoute(
    name: "adminproductedit",
    pattern: "admin/products/{id?}",
    defaults: new { controller = "Admin", action = "ProductEdit" }
);
app.MapControllerRoute(
    name: "admincategories",
    pattern: "admin/categories",
    defaults: new { controller = "Admin", action = "CategoryList" }
    );
app.MapControllerRoute(
    name: "admincategorycreate",
    pattern: "admin/categories/create",
    defaults: new { controller = "Admin", action = "CategoryCreate" }
    );
app.MapControllerRoute(
    name: "admincategorytedit",
    pattern: "admin/categories/{id?}",
    defaults: new { controller = "Admin", action = "CategoryEdit" }
);

app.MapControllerRoute(
    name: "products",
    pattern: "products/{category?}",
    defaults: new { controller = "Shop", action = "List" }
    );
app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Shop", action = "search" }
    );
app.MapControllerRoute(
    name: "productdetails",
    pattern: "{url}",
    defaults: new { controller = "Shop", action = "Details" }
    );


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await SeedIdentity.Seed(userManager, roleManager,configuration);
}

app.Run();
