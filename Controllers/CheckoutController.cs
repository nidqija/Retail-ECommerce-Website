using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Discounts;
using RetailECommerce.Services.Facades;
using RetailECommerce.Services.Observers;

namespace RetailECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CheckoutFacade _checkoutFacade;
        private readonly MyDbContext _context;
        private readonly IDiscountService _discountService;

        public CheckoutController(MyDbContext context, IDiscountService discountService)
        {
            _checkoutFacade = new CheckoutFacade();
            _context = context;
            _discountService = discountService;
        }

        private const string CartSessionKey = "ShoppingCart";

        private List<CartItem> GetCartItems()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void LoadCartData()
        {
            // Pull the real cart the shopper built (stored in session by CartController).
            var cartItems = GetCartItems();
            ViewBag.OrderItems = cartItems;

            decimal subtotal = cartItems.Sum(i => i.Price * i.Quantity);
            decimal tax = Math.Round(subtotal * 0.08m, 2);

            ViewBag.Subtotal = subtotal;
            ViewBag.Tax = tax;
            ViewBag.Total = subtotal + tax;

            // Discount codes the user can choose from on the order page.
            ViewBag.AvailableDiscounts = _discountService.GetAvailableDiscounts().ToList();
        }

        public IActionResult Index()
        {
            LoadCartData();
            return View();
        }

        [HttpPost]
        public IActionResult Process(string paymentType, decimal subtotal, string? discountCode)
        {
            LoadCartData();

            int userId = 1;
            int orderId = new Random().Next(1000, 9999);

            // Apply the chosen discount to the subtotal (before tax). Expired or
            // unknown codes are rejected and the full subtotal is charged.
            var discountResult = _discountService.ApplyDiscount(discountCode, subtotal);
            decimal payableSubtotal = discountResult.DiscountedSubtotal;

            // Build the item list from the shopper's actual cart.
            var cartItems = GetCartItems()
                .ToDictionary(i => i.Name, i => (object)i.Price);

            var checkoutResult = _checkoutFacade.ProcessCheckout(
                paymentType,
                payableSubtotal,
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
            ViewBag.PaymentNotification = notificationMessage;

            // Receipt figures, reflecting the discount cut-off.
            ViewBag.Subtotal = discountResult.OriginalSubtotal;
            ViewBag.DiscountApplied = discountResult.IsApplied;
            ViewBag.DiscountAmount = discountResult.DiscountAmount;
            ViewBag.DiscountMessage = discountResult.Message;
            ViewBag.DiscountCode = discountResult.Discount?.DiscountCode;
            ViewBag.Tax = Math.Round(payableSubtotal * 0.08m, 2);
            ViewBag.Total = checkoutResult.TotalAmount;

            return View("Process");
        }
    }
}