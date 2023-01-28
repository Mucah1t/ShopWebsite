using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductRepository, EfCoreProductRepository>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<ICategorryService, CategoryManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    SeedDatabase.Seed();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


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

app.Run();
