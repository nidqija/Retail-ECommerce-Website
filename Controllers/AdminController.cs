namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;

public class AdminController : Controller
{
    private readonly MyDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly RetailECommerce.Services.Facades.AdminDashboardFacade _dashboardFacade;

    public AdminController(MyDbContext context, IProductRepository productRepository, RetailECommerce.Services.Facades.AdminDashboardFacade dashboardFacade)
    {
        _context = context;
        _productRepository = productRepository;
        _dashboardFacade = dashboardFacade;
    }
    // GET: /Admin  — Admin hub / overview
    public IActionResult Index()
    {
        var summary = _dashboardFacade.GetDashboardSummary();
        ViewBag.DashboardSummary = summary;
        
        PageCreator pageCreator = new AdminHomePageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/Products  — product data table
    public IActionResult Products()
    {
        var products = _productRepository.GetAllProducts();
        PageCreator pageCreator = new AdminProductsPageCreator();
        ViewBag.Products = products;
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/CreateProduct  — empty create form
    public IActionResult CreateProduct()
    {
        PageCreator pageCreator = new AdminCreateProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/CreateProduct
    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.AddProduct(product);
            return RedirectToAction("Products");
        }
        PageCreator pageCreator = new AdminCreateProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/EditProduct/{id}
    public IActionResult EditProduct(int id)
    {
        var product = _productRepository.GetProductById(id);
        if (product == null) return NotFound();

        ViewBag.Product = product;
        PageCreator pageCreator = new AdminEditProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/DeleteProduct/{id}
    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        _productRepository.DeleteProduct(id);
        return RedirectToAction("Products");
    }

    // POST: /Admin/EditProduct/{id}
    [HttpPost]
    public IActionResult EditProduct(int id, Product product)
    {
        if (id != product.ProductId) return BadRequest();

        if (ModelState.IsValid)
        {
            _productRepository.UpdateProduct(product);
            return RedirectToAction("Products");
        }
        ViewBag.Product = product;
        PageCreator pageCreator = new AdminEditProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/Orders
    public IActionResult Orders()
    {
        var orders = _context.Orders.Include(p => p.User).ToList();
        ViewBag.Orders = orders;
        PageCreator pageCreator = new AdminOrdersPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/OrderDetails/{id}
    public IActionResult OrderDetails(int id)
    {
        var order = _context.Orders.Include(p => p.User).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();

        ViewBag.Order = order;
        PageCreator pageCreator = new AdminOrderDetailsPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/UpdateOrderStatus
    [HttpPost]
    public IActionResult UpdateOrderStatus(int id, string status)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.OrderStatus = status;
            _context.SaveChanges();
        }
        return RedirectToAction("OrderDetails", new { id });
    }

    // GET: /Admin/Enquiries
    public IActionResult Enquiries()
    {
        var enquiries = _context.Enquiries
            .Include(e => e.User)
            .Include(e => e.Product)
            .ToList();
            
        ViewBag.Enquiries = enquiries;
        PageCreator pageCreator = new EnquiriesPageCreator();
        return pageCreator.RenderPage(this);
    }
}