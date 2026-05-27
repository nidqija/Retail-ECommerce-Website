namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class ProductsController : Controller
{
    // GET: /Products  — product catalog grid
    public IActionResult Index(string searchKeyword = "", string category = "")
    {
        // Hardcoded mock products; replace with IProductRepository call later
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, full RGB backlight, detachable cable.",   Price = 89.99m,  StockQuantity = 42, Category = "Peripherals" },
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic shape, 3000 DPI, silent clicks.",                 Price = 39.99m,  StockQuantity = 78, Category = "Peripherals" },
            new Product { ProductId = 3, Name = "USB-C Hub (7-in-1)",   Description = "HDMI 4K, USB-A x3, SD card, PD 100W pass-through.",        Price = 29.99m,  StockQuantity = 5,  Category = "Accessories" },
            new Product { ProductId = 4, Name = "4K Monitor",           Description = "32-inch 4K display, 60Hz refresh rate, USB-C connectivity.", Price = 399.99m, StockQuantity = 12, Category = "Displays" },
            new Product { ProductId = 5, Name = "Laptop Stand",         Description = "Adjustable aluminum laptop stand for better ergonomics.",    Price = 24.99m,  StockQuantity = 35, Category = "Accessories" },
            new Product { ProductId = 6, Name = "Mechanical Gaming Mouse", Description = "Gaming-grade mouse with 10k DPI and RGB lighting.",        Price = 59.99m,  StockQuantity = 20, Category = "Peripherals" },
        };

        // Apply search filter by keyword
        if (!string.IsNullOrEmpty(searchKeyword))
        {
            products = products.Where(p => 
                p.Name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) || 
                p.Description.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        // Apply category filter
        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Get unique categories for the filter dropdown
        var allCategories = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, full RGB backlight, detachable cable.",   Price = 89.99m,  StockQuantity = 42, Category = "Peripherals" },
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic shape, 3000 DPI, silent clicks.",                 Price = 39.99m,  StockQuantity = 78, Category = "Peripherals" },
            new Product { ProductId = 3, Name = "USB-C Hub (7-in-1)",   Description = "HDMI 4K, USB-A x3, SD card, PD 100W pass-through.",        Price = 29.99m,  StockQuantity = 5,  Category = "Accessories" },
            new Product { ProductId = 4, Name = "4K Monitor",           Description = "32-inch 4K display, 60Hz refresh rate, USB-C connectivity.", Price = 399.99m, StockQuantity = 12, Category = "Displays" },
            new Product { ProductId = 5, Name = "Laptop Stand",         Description = "Adjustable aluminum laptop stand for better ergonomics.",    Price = 24.99m,  StockQuantity = 35, Category = "Accessories" },
            new Product { ProductId = 6, Name = "Mechanical Gaming Mouse", Description = "Gaming-grade mouse with 10k DPI and RGB lighting.",        Price = 59.99m,  StockQuantity = 20, Category = "Peripherals" },
        };
        
        var categories = allCategories.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();

        ViewBag.Products = products;
        ViewBag.Categories = categories;
        ViewBag.SearchKeyword = searchKeyword;
        ViewBag.SelectedCategory = category;
        
        PageCreator pageCreator = new ProductsIndexPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Products/Details/{id}
    public IActionResult Details(int id)
    {
        // Mock: return a product matching the id, or a fallback
        var product = new Product
        {
            ProductId  = id,
            Name       = $"Product #{id}",
            Description = "This is a detailed description of the product. It covers all key features, materials used, compatibility notes, and warranty information.",
            Price       = 49.99m + (id * 10),
            StockQuantity = id % 3 == 0 ? 0 : 15   // every 3rd item is "out of stock"
        };

        ViewBag.Product = product;
        PageCreator pageCreator = new ProductsDetailsPageCreator();
        return pageCreator.RenderPage(this);
    }
}
