//using PackageFoodManagementSystem.Repository.Data;
//using PackageFoodManagementSystem.Repository.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using PackageFoodManagementSystem.Repository.Interfaces;
//using PackageFoodManagementSystem.Services.Implementations;
//using PackageFoodManagementSystem.Services.Interfaces;
//using PackageFoodManagementSystem.Repository.Implementations;



//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//builder.Services.AddScoped<IBillRepository, BillRepository>();

//builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

//builder.Services.AddScoped<IOrderService, OrderService>();

//builder.Services.AddScoped<IBillingService, BillingService>();

//// --- 1. ADD SERVICES (Before builder.Build()) ---

//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



//// Move this HERE (above builder.Build)


//// DI: map interfaces to EFDB implementations
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();

//// ✅ Add authentication if you plan to use cookies
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Home/SignIn";
//        options.LogoutPath = "/Home/Logout";
//    });

//var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Services.Interfaces;

////// ✅ Global no-cache middleware
////app.Use(async (context, next) =>
////{
////    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
////    context.Response.Headers["Pragma"] = "no-cache";
////    context.Response.Headers["Expires"] = "0";
////    await next();
////});

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Welcome}/{id?}");

//app.Run();









// Program.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Implementations.PackageFoodManagementSystem.Repository.Implementations;
=========
﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
>>>>>>>>> Temporary merge branch 2
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Services.Interfaces;
<<<<<<<<< Temporary merge branch 1
// ... (Your existing usings)
=========


>>>>>>>>> Temporary merge branch 2

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DI Registrations
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Home/SignIn";
        options.LogoutPath = "/Home/Logout";
    });
// Register the Repository
// Register the Service 
// (Make sure the names match your files exactly, e.g., IProductService or ProductService)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register the Service (Mapping Interface to Implementation)
builder.Services.AddScoped<IProductService, ProductService>(); 
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ CRITICAL ORDER: Session -> Authentication -> Authorization
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Welcome}/{id?}");
app.Run();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();
