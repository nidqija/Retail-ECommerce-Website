using Microsoft.EntityFrameworkCore;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.Factory;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(connectionString));

// register the services in the dependency injection container to be used in the controllers
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPageRenderFactory, PageHandlerFactory>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Required for Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // How long the user stays logged in
    options.Cookie.HttpOnly = true; // Security: prevents JS from reading the cookie
    options.Cookie.IsEssential = true; // Required to work even if the user hasn't accepted cookies
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession(); // Enable session middleware

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();