using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Application.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ADD SERVICES (Before builder.Build()) ---

builder.Services.AddControllersWithViews();

// Move this HERE (above builder.Build)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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