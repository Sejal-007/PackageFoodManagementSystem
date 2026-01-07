//Initializes and configures the web application for Package Food Management System.

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllersWithViews();

//Finalizes the application setup,.

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())

{

    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios

    app.UseHsts();

}

app.UseHttpsRedirection();

// Serve static files from wwwroot (CSS, JS, images)

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Single, clean default route: Home/Index

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Welcome}/{id?}")

    .WithStaticAssets();


app.Run();

