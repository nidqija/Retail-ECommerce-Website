namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class AdminController : Controller
{
    // GET: /Admin  — Admin hub / overview
    public IActionResult Index()
    {
        PageCreator pageCreator = new AdminHomePageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/Products  — product data table
    public IActionResult Products()
    {
        // Hardcoded mock data; replace with DB call later
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, RGB backlit.", Price = 89.99m,  StockQuantity = 42 },
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic, 3000 DPI.",         Price = 39.99m,  StockQuantity = 78 },
            new Product { ProductId = 3, Name = "USB-C Hub (7-in-1)",   Description = "HDMI, USB-A, SD, PD.",        Price = 29.99m,  StockQuantity = 5  },
            new Product { ProductId = 4, Name = "27\" Monitor",          Description = "1440p IPS, 165 Hz.",          Price = 329.00m, StockQuantity = 12 },
        };

        PageCreator pageCreator = new AdminProductsPageCreator();
        // Pass model data via ViewBag so the Factory handler can still own view selection
        ViewBag.Products = products;
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/CreateProduct  — empty create form
    public IActionResult CreateProduct()
    {
        PageCreator pageCreator = new AdminCreateProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/CreateProduct  — stub: redirects back to Products table
    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        // Future: validate and save to DB via repository
        return RedirectToAction("Products");
    }

    // GET: /Admin/EditProduct/{id}  — pre-filled edit form
    public IActionResult EditProduct(int id)
    {
        // Mock single product; replace with DB lookup
        var product = new Product
        {
            ProductId = id,
            Name = "Sample Product",
            Description = "This is a mock product description for editing.",
            Price = 49.99m,
            StockQuantity = 20
        };

        ViewBag.Product = product;
        PageCreator pageCreator = new AdminEditProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/EditProduct/{id}  — stub: redirects back to Products table
    [HttpPost]
    public IActionResult EditProduct(int id, Product product)
    {
        // Future: validate and update in DB via repository
        return RedirectToAction("Products");
    }

    // GET: /Admin/Orders  — all orders table
    public IActionResult Orders()
    {
        // Hardcoded mock order data
        ViewBag.Orders = GetMockOrders();
        PageCreator pageCreator = new AdminOrdersPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/OrderDetails/{id}
    public IActionResult OrderDetails(int id)
    {
        // Return a single mock order by ID (just uses first mock for now)
        var order = GetMockOrders().FirstOrDefault(o => o.Id == id)
                    ?? GetMockOrders().First();
        ViewBag.Order = order;
        PageCreator pageCreator = new AdminOrderDetailsPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/UpdateOrderStatus  — stub status change
    [HttpPost]
    public IActionResult UpdateOrderStatus(int id, PaymentStatus status)
    {
        // Future: update status in DB
        return RedirectToAction("OrderDetails", new { id });
    }

    // --- Private helpers -------------------------------------------------

    private static List<Payment> GetMockOrders()
    {
        return new List<Payment>
        {
            new Payment { Id = 1, Total_Amount = 419.98m, PaymentDate = DateTime.Now.AddDays(-5),  PaymentStatus = PaymentStatus.Completed, PaymentMethod = PaymentMethod.CreditCard },
            new Payment { Id = 3, Total_Amount = 29.99m,  PaymentDate = DateTime.Now.AddDays(-1),  PaymentStatus = PaymentStatus.Failed,    PaymentMethod = PaymentMethod.CreditCard  },
            new Payment { Id = 4, Total_Amount = 658.97m, PaymentDate = DateTime.Now.AddHours(-3), PaymentStatus = PaymentStatus.Pending,   PaymentMethod = PaymentMethod.PayPal      },
        };
    }

    
}