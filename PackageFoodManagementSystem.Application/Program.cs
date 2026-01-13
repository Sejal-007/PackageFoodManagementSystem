using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository;
using PackageFoodManagementSystem.Repository.Data;
// Ensure both namespaces are present
using PackageFoodManagementSystem.Services;
using PackageFoodManagementSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ADD SERVICES ---
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. REGISTER DEPENDENCIES ---
// These must be defined before builder.Build()
builder.Services.AddScoped<
    PackageFoodManagementSystem.Services.Interfaces.IProductService,
    PackageFoodManagementSystem.Services.ProductService>();

var app = builder.Build();

// --- 3. CONFIGURE MIDDLEWARE ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();