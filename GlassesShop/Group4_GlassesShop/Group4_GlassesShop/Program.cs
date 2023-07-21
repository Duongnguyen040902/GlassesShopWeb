using DemoWebFirstMVCCore;
using Group4_GlassesShop.Areas.Admin.Models.Interface;
using Group4_GlassesShop.Areas.Admin.Models.Repository;
using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Repositories;
//using Group4_GlassesShop.Models.Interface;
//using Group4_GlassesShop.Models.ManagerProduct;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});
builder.Services.AddDistributedMemoryCache();
var connectionString = builder.Configuration.GetConnectionString("GlassesShopContext");
builder.Services.AddDbContext<GlassesShopContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<GlassesShopContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddScoped<IProduct, Products>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddMvc().AddSessionStateTempDataProvider();

builder.Services.AddScoped<IRepositoryOrder, RepositoryOrder>();
builder.Services.AddScoped<IBestSellerRespository, BestSellerRespository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRespository>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.AllowTrailingCommas = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
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

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
