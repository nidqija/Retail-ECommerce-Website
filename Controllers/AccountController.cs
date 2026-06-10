namespace RetailECommerce.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class AccountController : Controller
{
    private readonly MyDbContext _context;

    public AccountController(MyDbContext context)
    {
        _context = context;
    }

    // Resolve the logged-in user's id from their auth cookie / session,
    // falling back to the seeded demo user (1) when no one is signed in.
    private int CurrentUserId
    {
        get
        {
            var email = User.FindFirstValue(ClaimTypes.Name)
                        ?? HttpContext.Session.GetString("UserEmail");

            if (!string.IsNullOrEmpty(email))
            {
                var userId = _context.Users
                    .Where(u => u.Email == email)
                    .Select(u => (int?)u.UserId)
                    .FirstOrDefault();

                if (userId.HasValue)
                {
                    return userId.Value;
                }
            }

            return 1;
        }
    }

    // GET: /Account/Orders  — customer order history (real orders from the DB)
    public IActionResult Orders()
    {
        int userId = CurrentUserId;

        var orders = _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                OrderId = o.Id,
                Date = o.OrderDate,
                Total = o.TotalAmount,
                Status = o.OrderStatus
            })
            .ToList();

        ViewBag.Orders = orders;

        PageCreator pageCreator = new AccountOrdersPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Account/OrderDetail/{orderId} — detailed view of a real order
    public IActionResult OrderDetail(int orderId)
    {
        int userId = CurrentUserId;

        // Load the order with its line items + product names. Scoped to the
        // current user so people can't view someone else's order by guessing ids.
        var o = _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(x => x.Id == orderId && x.UserId == userId);

        if (o == null)
        {
            ViewBag.Order = null;
        }
        else
        {
            var order = new
            {
                OrderId = o.Id,
                Date = o.OrderDate,
                Total = o.TotalAmount,
                Status = o.OrderStatus,
                PaymentMethod = o.PaymentMethod,
                EstimatedDelivery = (DateTime?)o.OrderDate.AddDays(5),
                TrackingNumber = (string?)null,
                Subtotal = (decimal?)o.Subtotal,
                Tax = (decimal?)o.Tax,
                Shipping = (decimal?)0m,
                Items = o.OrderItems.Select(i => new
                {
                    ProductName = i.Product != null ? i.Product.Name : $"Product #{i.ProductId}",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.UnitPrice * i.Quantity
                }).ToList()
            };

            ViewBag.Order = order;
        }

        PageCreator pageCreator = new AccountOrderDetailPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
