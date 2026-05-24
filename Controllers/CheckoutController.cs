namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Strategy.Payment;


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
        return View();  
    }
// ADD THIS NEW METHOD TO HANDLE THE FORM SUBMISSION
    // POST: /Checkout/Process
    [HttpPost]
    public IActionResult Process(string paymentType, decimal totalAmount)
    {
        var checkoutContext = new CheckoutContext();

        // Select the strategy based on user input from the checkout form
        switch (paymentType?.ToLower())
        {
            case "card":
                checkoutContext.SetPaymentStrategy(new CardPayment());
                break;
            case "qr":
                checkoutContext.SetPaymentStrategy(new QRPayment());
                break;
            case "cod":
                checkoutContext.SetPaymentStrategy(new CashOnDelivery());
                break;
            default:
                ModelState.AddModelError("", "Invalid payment method selected.");
                return View("Index");
        }

        try
        {
            // Execute the chosen strategy
            bool isSuccess = checkoutContext.ExecutePayment(totalAmount);

            if (isSuccess)
            {
                ViewBag.Message = $"Payment of {totalAmount:C} via {paymentType.ToUpper()} processed successfully!";
                
                // For now, return a basic Content result or create a Success.cshtml view later
                return Content($"Success! {ViewBag.Message}"); 
            }
            else
            {
                ViewBag.Message = "Payment failed. Please try again.";
                return View("Index");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Index");
        }
    }
}
