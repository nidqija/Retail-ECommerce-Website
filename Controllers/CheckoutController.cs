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

        // NOTE: This codebase identifies the shopper as user 1 throughout
        // (see DataSeeder). Centralised here so it's easy to swap for the
        // real logged-in user id later.
        private int CurrentUserId => 1;

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

            // Discounts this user has already redeemed - the view renders these
            // as disabled so they can't be picked again.
            ViewBag.UsedDiscountIds = _discountService.GetUsedDiscountIds(CurrentUserId).ToHashSet();
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

            int userId = CurrentUserId;
            int orderId = new Random().Next(1000, 9999);

            // Apply the chosen discount to the subtotal (before tax). Expired,
            // unknown, or already-used codes are rejected and the full subtotal
            // is charged.
            var discountResult = _discountService.ApplyDiscount(discountCode, subtotal, userId);
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

            // The order went through (paid now, or to be paid on delivery), so
            // mark the discount as used by this user. It will then show up as
            // disabled the next time they reach checkout.
            if (discountResult.IsApplied
                && discountResult.Discount != null
                && paymentStatus != "Failed")
            {
                _discountService.RecordDiscountUsed(userId, discountResult.Discount.Id);
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