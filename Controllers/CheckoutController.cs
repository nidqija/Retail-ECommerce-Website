using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Facades;
using System;

namespace RetailECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CheckoutFacade _checkoutFacade;

        public CheckoutController()
        {
            _checkoutFacade = new CheckoutFacade();
        }

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
        public IActionResult Process(string paymentType, decimal subtotal)
        {
            LoadCartData();

            // Prepare cart items for notifications
            var cartItems = new Dictionary<string, object>
            {
                { "Mechanical Keyboard", 89.99m },
                { "27\" IPS Monitor", 329.00m }
            };

            // Use the facade to process the entire checkout
            var checkoutResult = _checkoutFacade.ProcessCheckout(
                paymentType,
                subtotal,
                orderId: new Random().Next(1000, 9999), // Mock order ID
                userId: 1, // Mock user ID
                cartItems);

            if (checkoutResult.IsSuccessful)
            {
                ViewBag.Message = checkoutResult.GetDisplayMessage();
                ViewBag.TransactionId = checkoutResult.GetTransactionId();
                ViewBag.OrderStatus = checkoutResult.GetOrderStatus();
                ViewBag.PaymentType = paymentType;
                ViewBag.Total = checkoutResult.TotalAmount;
                return View("Process");
            }
            else
            {
                ModelState.AddModelError("", checkoutResult.Message);
                return View("Index");
            }
        }
    }
}