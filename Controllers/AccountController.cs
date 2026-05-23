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

    // POST: /Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
