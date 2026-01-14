using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- 1. DATABASE CONFIGURATION ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. DEPENDENCY INJECTION (DI) REGISTRATIONS ---

// Batch Management
builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<IBatchService, BatchService>();

// Order Management
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Billing & Payments
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// User Management
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// --- 3. AUTHENTICATION & UI ---
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/SignIn";
        options.LogoutPath = "/Home/Logout";
    });

var app = builder.Build();

// --- 4. MIDDLEWARE PIPELINE ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();