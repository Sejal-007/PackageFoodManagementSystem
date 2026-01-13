using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Services.Implementations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IBillRepository, BillRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IBillingService, BillingService>();

// --- 1. ADD SERVICES (Before builder.Build()) ---

builder.Services.AddControllersWithViews();




// Move this HERE (above builder.Build)


// --- 2. BUILD THE APP ---

var app = builder.Build();

// --- 3. CONFIGURE MIDDLEWARE (After builder.Build()) ---

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
    pattern: "{controller=Home}/{action=Welcome}/{id?}")
    .WithStaticAssets();

app.Run();