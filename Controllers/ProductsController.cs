namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Data;
using RetailECommerce.ViewModels;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;


public class ProductsController : Controller
{

    private IProductRepository _productRepository;
    private IEnquiryRepository _enquiryRepository;

    private readonly MyDbContext _context;


    public ProductsController(IProductRepository productRepository , IEnquiryRepository enquiryRepository, MyDbContext context)
    {
        _productRepository = productRepository;
        _enquiryRepository = enquiryRepository;
        _context = context;
    }
    // GET: /Products  — product catalog grid
    public IActionResult Index(string searchKeyword = "", string category = "", string subCategory = "")
    {
        // Hardcoded mock products; replace with IProductRepository call later
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, full RGB backlight, detachable cable.",   Price = 89.99m,  StockQuantity = 42, Category = "Computers & Accessories", SubCategory = "Technology"},
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic shape, 3000 DPI, silent clicks.",                 Price = 39.99m,  StockQuantity = 78, Category = "Computers & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 3, Name = "Madrid shirt signed by Messi",   Description = "\"Authentic\" jersey signed by Lionel Messi.",     Price = 9.99m,  StockQuantity = 5,  Category = "Men Clothes", SubCategory = "Apparel" },
            new Product { ProductId = 4, Name = "Basic dress",          Description = "Basic dress for everyday wear.",                             Price = 19.99m,  StockQuantity = 5,  Category = "Women Clothes", SubCategory = "Apparel" },
            new Product { ProductId = 5, Name = "4K Monitor",           Description = "32-inch 4K display, 60Hz refresh rate, USB-C connectivity.", Price = 399.99m, StockQuantity = 12, Category = "Computers & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 6, Name = "Playstation 10 (PSX)",  Description = "Latest gaming console with enhanced graphics.",              Price = 2499.99m,  StockQuantity = 35, Category = "Gaming & Consoles", SubCategory = "Entertainment" },
            new Product { ProductId = 7, Name = "Iphone 9999",          Description = "The latest model, iphone 9999 that can cure all diseases.",  Price = 9999.99m,  StockQuantity = 10, Category = "Mobile & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 8, Name = "Bluetooth Speaker",    Description = "Portable speaker with deep bass and 12-hour battery life.",   Price = 59.99m,  StockQuantity = 25, Category = "Audio & Headphones", SubCategory = "Technology" },
            new Product { ProductId = 9, Name = "Sport Shoes",             Description = "Comfortable shoes for sports activities.",        Price = 29.99m,  StockQuantity = 50, Category = "Sports & Outdoors", SubCategory = "Apparel" },
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

        // Apply sub-category filter
        if (!string.IsNullOrEmpty(subCategory))
        {
            products = products.Where(p => p.SubCategory.Equals(subCategory, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        var allCategories = new List<Product>
        {
            new Product { ProductId = 1, Name = "Mechanical Keyboard",  Description = "Tactile switches, full RGB backlight, detachable cable.",   Price = 89.99m,  StockQuantity = 42, Category = "Computers & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 2, Name = "Wireless Mouse",       Description = "Ergonomic shape, 3000 DPI, silent clicks.",                 Price = 39.99m,  StockQuantity = 78, Category = "Computers & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 3, Name = "Madrid shirt signed by Messi",   Description = "\"Authentic\" jersey signed by Lionel Messi.",     Price = 9.99m,  StockQuantity = 5,  Category = "Men Clothes", SubCategory = "Apparel" },
            new Product { ProductId = 4, Name = "Basic dress",          Description = "Basic dress for everyday wear.",                             Price = 19.99m,  StockQuantity = 5,  Category = "Women Clothes", SubCategory = "Apparel" },
            new Product { ProductId = 5, Name = "4K Monitor",           Description = "32-inch 4K display, 60Hz refresh rate, USB-C connectivity.", Price = 399.99m, StockQuantity = 12, Category = "Computers & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 6, Name = "Playstation 10 (PSX)",  Description = "Latest gaming console with enhanced graphics.",              Price = 2499.99m,  StockQuantity = 35, Category = "Gaming & Consoles", SubCategory = "Entertainment" },
            new Product { ProductId = 7, Name = "Iphone 9999",          Description = "The latest model, iphone 9999 that can cure all diseases.",        Price = 9999.99m,  StockQuantity = 10, Category = "Mobile & Accessories", SubCategory = "Technology" },
            new Product { ProductId = 8, Name = "Bluetooth Speaker",    Description = "Portable speaker with deep bass and 12-hour battery life.",   Price = 59.99m,  StockQuantity = 25, Category = "Audio & Headphones", SubCategory = "Technology" },
            new Product { ProductId = 9, Name = "Sport Shoes",             Description = "Comfortable shoes for sports activities.",        Price = 29.99m,  StockQuantity = 50, Category = "Sports & Outdoors", SubCategory = "Apparel" },
        };
        
        var categories = allCategories.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
        var subCategories = allCategories.Select(p => p.SubCategory).Distinct().OrderBy(sc => sc).ToList();

        ViewBag.Products = products;
        ViewBag.Categories = categories;
        ViewBag.SubCategories = subCategories;
        ViewBag.SearchKeyword = searchKeyword;
        ViewBag.SelectedCategory = category;
        ViewBag.SelectedSubCategory = subCategory;
        PageCreator pageCreator = new ProductsIndexPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Products/Details/{id}
    //[Authorize] // only authenticated users can access the product details page
    public IActionResult Details(int id)
    {
        // Mock: return a product matching the id, or a fallback

        // update : replace the mock data with the data from the database using the repository pattern
        var productbyId = _productRepository.GetProductById(id);
        ViewBag.Product = productbyId;

        var enquriesbyId = _enquiryRepository.GetAllEnquiries().Where(e => e.ProductId == id).ToList();
        ViewBag.Enquiries = enquriesbyId;

        var reviewsById = _context.Reviews
            .Where(r => r.ProductId == id)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        ViewBag.Reviews = reviewsById;

        PageCreator pageCreator = new ProductsDetailsPageCreator();
        return pageCreator.RenderPage(this);
    }

    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitReview(SubmitReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ReviewError"] = "Please select a rating and keep your comment within 500 characters.";
            return RedirectToAction("Details", new { id = model.ProductId });
        }

        int userId = 1;

        var review = new Review
        {
            ProductId = model.ProductId,
            UserId = userId,
            Rating = model.Rating,
            Comment = model.Comment ?? string.Empty,
            CreatedAt = DateTime.Now
        };

        _context.Reviews.Add(review);
        _context.SaveChanges();

        TempData["ReviewMessage"] = "Your feedback and review has been submitted.";
        return RedirectToAction("Details", new { id = model.ProductId });
    }

    
}
