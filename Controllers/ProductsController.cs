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
private readonly IReviewRepository _reviewRepository;

public ProductsController(
    IProductRepository productRepository,
    IEnquiryRepository enquiryRepository,
    IReviewRepository reviewRepository,
    MyDbContext context)
{
    _productRepository = productRepository;
    _enquiryRepository = enquiryRepository;
    _reviewRepository = reviewRepository;
    _context = context;
}

    // GET: /Products  — product catalog grid
    public IActionResult Index(string searchKeyword = "", string category = "", string subCategory = "")
    {
        // Real catalog from the database (via the repository). Every product
        // shown here therefore exists in the DB, so View Details / Add to Cart
        // for it will always resolve.
        var allProducts = _productRepository.GetAllProducts().ToList();

        var products = allProducts;

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

        // Filter options are derived from the full (unfiltered) catalog.
        var categories = allProducts
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .OrderBy(c => c)
            .ToList();
        var subCategories = allProducts
            .Select(p => p.SubCategory)
            .Where(sc => !string.IsNullOrEmpty(sc))
            .Distinct()
            .OrderBy(sc => sc)
            .ToList();

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

        var reviewsById = _reviewRepository.GetAllReviews()
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
