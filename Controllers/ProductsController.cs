namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class ProductsController : Controller
{
    // GET: /Products  — product catalog grid
    public IActionResult Index()
    {
        // Hardcoded mock products; replace with IProductRepository call later
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, full RGB backlight, detachable cable.",   Price = 89.99m,  StockQuantity = 42 },
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic shape, 3000 DPI, silent clicks.",                 Price = 39.99m,  StockQuantity = 78 },
            new Product { ProductId = 3, Name = "USB-C Hub (7-in-1)",   Description = "HDMI 4K, USB-A x3, SD card, PD 100W pass-through.",        Price = 29.99m,  StockQuantity = 5  },

        };

        ViewBag.Products = products;
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
