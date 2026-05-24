using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Strategy.Payment;
using System;

namespace RetailECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        // Helper method to load mock cart data so we don't repeat code
        private void LoadCartData()
        {
            ViewBag.OrderItems = new[]
            {
                new { Name = "Mechanical Keyboard", Price = 89.99m,  Quantity = 1 },
                new { Name = "27\" IPS Monitor",    Price = 329.00m, Quantity = 1 },
            };
            ViewBag.Subtotal = 418.99m;
            ViewBag.Tax = Math.Round(418.99m * 0.08m, 2);
            ViewBag.Total = ViewBag.Subtotal + ViewBag.Tax;
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            LoadCartData();
            return View();  
        }

        // POST: /Checkout/Process
        [HttpPost]
        public IActionResult Process(string paymentType, decimal totalAmount)
        {
            var checkoutContext = new CheckoutContext();

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
                    LoadCartData(); // Reload cart data before returning to view
                    return View("Index");
            }

            try
            {
                bool isSuccess = checkoutContext.ExecutePayment(totalAmount);

                if (isSuccess)
                {
                    ViewBag.Message = $"Payment of {totalAmount:C} via {paymentType.ToUpper()} processed successfully!";
                    return View("Process"); // Assuming you created the Process.cshtml view we discussed earlier
                }
                else
                {
                    ViewBag.Message = "Payment failed. Please try again.";
                    LoadCartData(); // Reload cart data before returning to view
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                LoadCartData(); // Reload cart data before returning to view
                return View("Index");
            }
        }
    }
}