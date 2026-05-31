using Microsoft.EntityFrameworkCore;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.Factory;
using RetailECommerce.Data;
using RetailECommerce.Services.Strategy.Report;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(connectionString));

// register the services in the dependency injection container to be used in the controllers
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEnquiryRepository, EnquiryRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportStrategy, PDFReportStrategy>();
builder.Services.AddScoped<IReportStrategy, CSVReportStrategy>();
builder.Services.AddScoped<ReportContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Required for Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // How long the user stays logged in
    options.Cookie.HttpOnly = true; // Security: prevents JS from reading the cookie
    options.Cookie.IsEssential = true; // Required to work even if the user hasn't accepted cookies

   
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie( options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/SignIn/Index"; // Redirect to this path if not authenticated
    options.AccessDeniedPath = "/SignIn/Index"; // Redirect to this path if access is denied
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

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// seed the database with a default admin user if no users exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    try
    {
        // seed the database with a default admin user if no users exist
        var context = services.GetRequiredService<MyDbContext>();

        await DataSeeder.SeedAdminAsync(context);
        await DataSeeder.SeedProductAsync(context);
        await DataSeeder.SeedPaymentAsync(context);
        await DataSeeder.SeedEnquiryAsync(context);
        
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

app.Run();