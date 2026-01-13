using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PackageFoodManagementSystem.Repository.Interface;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Services.Interface;
using PackageFoodManagementSystem.Repository.Implementations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI: map interfaces to EFDB implementations
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// ✅ Add authentication if you plan to use cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/SignIn";
        options.LogoutPath = "/Home/Logout";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//// ✅ Global no-cache middleware
//app.Use(async (context, next) =>
//{
//    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
//    context.Response.Headers["Pragma"] = "no-cache";
//    context.Response.Headers["Expires"] = "0";
//    await next();
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();
