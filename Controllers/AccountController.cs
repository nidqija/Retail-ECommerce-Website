namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class AccountController : Controller
{
    // GET: /Account/Orders  — customer order history
    public IActionResult Orders()
    {
        // Mock past orders; replace with DB query filtered by session UserEmail later
        var orders = new[]
        {
            new { OrderId = 1001, Date = DateTime.Now.AddDays(-30), Total = 419.98m, Status = PaymentStatus.Completed },

            new { OrderId = 1004, Date = DateTime.Now.AddDays(-1),  Total = 658.97m, Status = PaymentStatus.Pending   },
        };

        ViewBag.Orders = orders;

        PageCreator pageCreator = new AccountOrdersPageCreator();
        return pageCreator.RenderPage(this);
    }

    // GET: /Account/OrderDetail/{orderId} — view detailed order status and items
    public IActionResult OrderDetail(int orderId)
    {
        // Mock order detail; replace with actual DB query by orderId later
        var mockOrders = new[]
        {
            new {
                OrderId = 1001,
                Date = DateTime.Now.AddDays(-30),
                Total = 419.98m,
                Status = PaymentStatus.Completed,
                PaymentMethod = "Credit Card",
                EstimatedDelivery = DateTime.Now.AddDays(-25),
                TrackingNumber = "TRK-2026-001-9876",
                Subtotal = 389.98m,
                Tax = 30.00m,
                Shipping = 0m,
                Items = new[]
                {
                    new { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 89.99m, Subtotal = 89.99m },
                    new { ProductName = "27\" IPS Monitor", Quantity = 1, UnitPrice = 329.00m, Subtotal = 329.00m }
                }
            },
            new {
                OrderId = 1004,
                Date = DateTime.Now.AddDays(-1),
                Total = 658.97m,
                Status = PaymentStatus.Pending,
                PaymentMethod = "QR Payment",
                EstimatedDelivery = DateTime.Now.AddDays(5),
                TrackingNumber = (string)null,
                Subtotal = 608.97m,
                Tax = 50.00m,
                Shipping = 0m,
                Items = new[]
                {
                    new { ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 29.99m, Subtotal = 59.98m },
                    new { ProductName = "USB-C Hub", Quantity = 1, UnitPrice = 49.99m, Subtotal = 49.99m },
                    new { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 89.99m, Subtotal = 89.99m },
                    new { ProductName = "Monitor Stand", Quantity = 1, UnitPrice = 39.99m, Subtotal = 39.99m },
                    new { ProductName = "Desk Lamp", Quantity = 1, UnitPrice = 35.99m, Subtotal = 35.99m },
                    new { ProductName = "Keyboard Pad", Quantity = 1, UnitPrice = 24.99m, Subtotal = 24.99m }
                }
            }
        };

        var order = mockOrders.FirstOrDefault(o => o.OrderId == orderId);
        
        if (order == null)
        {
            ViewBag.Order = null;
        }
        else
        {
            ViewBag.Order = order;
        }

        PageCreator pageCreator = new AccountOrderDetailsPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
