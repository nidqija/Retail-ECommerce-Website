using Microsoft.EntityFrameworkCore;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.Factory;
using RetailECommerce.Data;
using RetailECommerce.Services.Strategy.Report;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 1. Configure Database Context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Register Application Services (Dependency Injection)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEnquiryRepository, EnquiryRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<RetailECommerce.Services.Discounts.IDiscountService, RetailECommerce.Services.Discounts.DiscountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportStrategy, PDFReportStrategy>();
builder.Services.AddScoped<IReportStrategy, CSVReportStrategy>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ReportContext>();
builder.Services.AddScoped<RetailECommerce.Services.Facades.AdminDashboardFacade>();
builder.Services.AddScoped<RetailECommerce.Services.Observers.NotificationSubject>();
builder.Services.AddScoped<RetailECommerce.Services.Observers.AdminNotificationObserver>();
builder.Services.AddScoped<RetailECommerce.Services.Observers.CustomerNotificationObserver>();
builder.Services.AddSingleton<RetailECommerce.Services.Payment.IQrCodeDetector, RetailECommerce.Services.Payment.ZXingQrCodeDetector>();

builder.Services.AddControllersWithViews();

// 3. Configure Session & Cache
// NOTE: AddDistributedMemoryCache stores sessions in volatile server memory. 
// Restarting the application automatically destroys all active session data.
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;                 
    options.Cookie.IsEssential = true;             
});

// 4. Configure Authentication Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        options.LoginPath = "/SignIn/Index";       
        options.AccessDeniedPath = "/SignIn/Index"; 
    });

// 5. Trust the hosting platform's reverse proxy (Render terminates TLS at its edge
// and forwards plain HTTP with X-Forwarded-Proto). Without this, UseHttpsRedirection
// sees an http request, redirects to https, and the proxy loops it back forever.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // The proxy's IP isn't known ahead of time, so no allowlist.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// ==========================================
// STARTUP TASKS (Cache Cleanup & DB Seeding)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // Optional: Explicit Cache Access on Boot
    try
    {
        var cache = services.GetService<IDistributedCache>();
        // If you migrate to a persistent cache (Redis/SQL) later, 
        // you would run explicit removal keys or flush commands here.
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while inspecting the cache: {ex.Message}");
    }

    // Seed the database with default records if empty
    try
    {
        var context = services.GetRequiredService<MyDbContext>();

        await DataSeeder.SeedAdminAsync(context);
        await DataSeeder.SeedProductAsync(context);
        await DataSeeder.SeedPaymentAsync(context);
        await DataSeeder.SeedEnquiryAsync(context);
        await DataSeeder.SeedReviewAsync(context);
        await DataSeeder.SeedOrderAsync(context);
        await DataSeeder.SeedDiscountAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

// ==========================================
// MIDDLEWARE PIPELINE
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Must run before any middleware that inspects the request scheme.
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseRouting();

// Middleware Order is critical: Session -> Authentication -> Authorization
app.UseSession();        
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();