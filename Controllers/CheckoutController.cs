using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Facades;
using RetailECommerce.Services.Observers;

namespace RetailECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CheckoutFacade _checkoutFacade;
        private readonly MyDbContext _context;

        public CheckoutController(MyDbContext context)
        {
            _checkoutFacade = new CheckoutFacade();
            _context = context;
        }

        private void LoadCartData()
        {
            ViewBag.OrderItems = new[]
            {
                new { Name = "Mechanical Keyboard", Price = 89.99m, Quantity = 1 },
                new { Name = "27\" IPS Monitor", Price = 329.00m, Quantity = 1 },
            };

            ViewBag.Subtotal = 418.99m;
            ViewBag.Tax = Math.Round(418.99m * 0.08m, 2);
            ViewBag.Total = ViewBag.Subtotal + ViewBag.Tax;
        }

        public IActionResult Index()
        {
            LoadCartData();
            return View();
        }

        [HttpPost]
        public IActionResult Process(string paymentType, decimal subtotal)
        {
            LoadCartData();

            int userId = 1;
            int orderId = new Random().Next(1000, 9999);

           
            var cartItems = new Dictionary<string, object>
            {
                { "Mechanical Keyboard", 89.99m },
                { "27\" IPS Monitor", 329.00m }
            };

            var checkoutResult = _checkoutFacade.ProcessCheckout(
                paymentType,
                subtotal,
                orderId,
                userId,
                cartItems
            );

            string paymentStatus;
            string notificationMessage;

            if (paymentType?.ToLower() == "cod")
            {
                paymentStatus = "Pending";
                notificationMessage =
                    $"Payment pending. Order #{orderId} has been placed, but payment will be collected during delivery.";
            }
            else if (checkoutResult.IsSuccessful)
            {
                paymentStatus = "Successful";
                notificationMessage =
                    $"Payment successful. Your order #{orderId} has been placed. Transaction ID: {checkoutResult.GetTransactionId()}";
            }
            else
            {
                paymentStatus = "Failed";
                notificationMessage =
                    $"Payment failed. Order #{orderId} was not placed. Reason: {checkoutResult.Message}";
            }

            var notification = new Notification
            {
                UserId = userId,
                Message = notificationMessage,
                Type = NotificationType.PaymentUpdate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.IsPaymentSuccessful = paymentStatus == "Successful";
            ViewBag.IsPaymentPending = paymentStatus == "Pending";
            ViewBag.IsPaymentFailed = paymentStatus == "Failed";

            ViewBag.Message = checkoutResult.GetDisplayMessage();
            ViewBag.TransactionId = checkoutResult.GetTransactionId();
            ViewBag.OrderStatus = paymentStatus;
            ViewBag.PaymentType = paymentType;
            ViewBag.Total = checkoutResult.TotalAmount;
            ViewBag.PaymentNotification = notificationMessage;

            return View("Process");
        }
    }
}