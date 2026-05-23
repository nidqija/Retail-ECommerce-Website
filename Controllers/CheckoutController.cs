namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;


public class CheckoutController : Controller
{
    // GET: /Checkout
    public IActionResult Index()
    {
        // Mock order summary to display in the sidebar
        ViewBag.OrderItems = new[]
        {
            new { Name = "Mechanical Keyboard", Price = 89.99m,  Quantity = 1 },
            new { Name = "27\" IPS Monitor",    Price = 329.00m, Quantity = 1 },
        };
        ViewBag.Subtotal = 418.99m;
        ViewBag.Tax      = Math.Round(418.99m * 0.08m, 2);  // 8% mock tax
        ViewBag.Total    = ViewBag.Subtotal + ViewBag.Tax;

        PageCreator pageCreator = new CheckoutPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Checkout/PlaceOrder  — stub: redirects to Home with a success flag
    [HttpPost]
    public IActionResult PlaceOrder()
    {
        // Future: validate forms, create Payment record, clear cart
        TempData["OrderSuccess"] = true;
        return RedirectToAction("Index", "Home");
    }
}
