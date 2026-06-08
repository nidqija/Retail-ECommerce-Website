namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.Logging;

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
        AdminLogger.Instance.Log("AdminController constructor: Initialized with dependencies.");
    }
    // GET: /Admin  — Admin hub / overview
    public IActionResult Index()
    {
        AdminLogger.Instance.Log("AdminController.Index: Entering Index page. Fetching dashboard summary statistics.");
        var summary = _dashboardFacade.GetDashboardSummary();
        ViewBag.DashboardSummary = summary;
        
        PageCreator pageCreator = new AdminHomePageCreator();
        AdminLogger.Instance.Log("AdminController.Index: Rendering home page using AdminHomePageCreator.");
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/Products  — product data table
    public IActionResult Products()
    {
        AdminLogger.Instance.Log("AdminController.Products: Entering Products page. Fetching all products from repository.");
        var products = _productRepository.GetAllProducts();
        PageCreator pageCreator = new AdminProductsPageCreator();
        ViewBag.Products = products;
        AdminLogger.Instance.Log("AdminController.Products: Rendering products page.");
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/CreateProduct  — empty create form
    public IActionResult CreateProduct()
    {
        AdminLogger.Instance.Log("AdminController.CreateProduct [GET]: Displaying empty create product form.");
        PageCreator pageCreator = new AdminCreateProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/CreateProduct
    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        AdminLogger.Instance.Log($"AdminController.CreateProduct [POST]: Request received to create product. Name: '{product?.Name}', Price: {product?.Price}.");
        if (ModelState.IsValid)
        {
            _productRepository.AddProduct(product);
            AdminLogger.Instance.Log($"AdminController.CreateProduct [POST]: Product '{product.Name}' successfully created. Redirecting to Products list.");
            return RedirectToAction("Products");
        }
        AdminLogger.Instance.Log("AdminController.CreateProduct [POST]: ModelState is invalid. Reloading form with validation messages.");
        PageCreator pageCreator = new AdminCreateProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/EditProduct/{id}
    public IActionResult EditProduct(int id)
    {
        AdminLogger.Instance.Log($"AdminController.EditProduct [GET]: Fetching product with ID: {id} for editing.");
        var product = _productRepository.GetProductById(id);
        if (product == null)
        {
            AdminLogger.Instance.Log($"AdminController.EditProduct [GET]: Product with ID: {id} not found. Returning 404 NotFound.");
            return NotFound();
        }

        ViewBag.Product = product;
        PageCreator pageCreator = new AdminEditProductPageCreator();
        AdminLogger.Instance.Log($"AdminController.EditProduct [GET]: Product ID: {id} found ('{product.Name}'). Rendering edit page.");
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/DeleteProduct/{id}
    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        AdminLogger.Instance.Log($"AdminController.DeleteProduct [POST]: Request received to delete product ID: {id}.");
        _productRepository.DeleteProduct(id);
        AdminLogger.Instance.Log($"AdminController.DeleteProduct [POST]: Product ID: {id} successfully deleted. Redirecting to Products list.");
        return RedirectToAction("Products");
    }

    // POST: /Admin/EditProduct/{id}
    [HttpPost]
    public IActionResult EditProduct(int id, Product product)
    {
        AdminLogger.Instance.Log($"AdminController.EditProduct [POST]: Request received to update product ID: {id}. Name: '{product?.Name}', Price: {product?.Price}.");
        if (id != product.ProductId)
        {
            AdminLogger.Instance.Log($"AdminController.EditProduct [POST]: ID mismatch. Route ID: {id}, Product ID: {product?.ProductId}. Returning 400 BadRequest.");
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _productRepository.UpdateProduct(product);
            AdminLogger.Instance.Log($"AdminController.EditProduct [POST]: Product ID: {id} successfully updated. Redirecting to Products list.");
            return RedirectToAction("Products");
        }
        AdminLogger.Instance.Log($"AdminController.EditProduct [POST]: ModelState is invalid for product ID: {id}. Reloading edit form.");
        ViewBag.Product = product;
        PageCreator pageCreator = new AdminEditProductPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/Orders
    public IActionResult Orders()
    {
        AdminLogger.Instance.Log("AdminController.Orders: Fetching list of all orders including user details.");
        var orders = _context.Orders.Include(p => p.User).ToList();
        ViewBag.Orders = orders;
        PageCreator pageCreator = new AdminOrdersPageCreator();
        AdminLogger.Instance.Log($"AdminController.Orders: Loaded {orders.Count} orders. Rendering orders page.");
        return pageCreator.RenderPage(this);
    }

    // GET: /Admin/OrderDetails/{id}
    public IActionResult OrderDetails(int id)
    {
        AdminLogger.Instance.Log($"AdminController.OrderDetails [GET]: Fetching details for order ID: {id}.");
        var order = _context.Orders.Include(p => p.User).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            AdminLogger.Instance.Log($"AdminController.OrderDetails [GET]: Order ID: {id} not found. Returning 404 NotFound.");
            return NotFound();
        }

        ViewBag.Order = order;
        PageCreator pageCreator = new AdminOrderDetailsPageCreator();
        AdminLogger.Instance.Log($"AdminController.OrderDetails [GET]: Order ID: {id} details retrieved successfully. Rendering details page.");
        return pageCreator.RenderPage(this);
    }

    // POST: /Admin/UpdateOrderStatus
    [HttpPost]
    public IActionResult UpdateOrderStatus(int id, string status)
    {
        AdminLogger.Instance.Log($"AdminController.UpdateOrderStatus [POST]: Request to update order ID: {id} to status: '{status}'.");
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            string oldStatus = order.OrderStatus;
            order.OrderStatus = status;
            _context.SaveChanges();
            AdminLogger.Instance.Log($"AdminController.UpdateOrderStatus [POST]: Order ID: {id} status updated from '{oldStatus}' to '{status}'.");
        }
        else
        {
            AdminLogger.Instance.Log($"AdminController.UpdateOrderStatus [POST]: Order ID: {id} not found. No updates made.");
        }
        return RedirectToAction("OrderDetails", new { id });
    }

    // GET: /Admin/Enquiries
    public IActionResult Enquiries()
    {
        AdminLogger.Instance.Log("AdminController.Enquiries: Fetching list of all enquiries including user and product details.");
        var enquiries = _context.Enquiries
            .Include(e => e.User)
            .Include(e => e.Product)
            .ToList();
            
        ViewBag.Enquiries = enquiries;
        PageCreator pageCreator = new EnquiriesPageCreator();
        AdminLogger.Instance.Log($"AdminController.Enquiries: Loaded {enquiries.Count} enquiries. Rendering enquiries page.");
        return pageCreator.RenderPage(this);
    }
}