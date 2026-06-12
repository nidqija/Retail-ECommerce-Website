using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Discounts;
using RetailECommerce.Services.Facades;
using RetailECommerce.Services.Observers;
using RetailECommerce.Services.Payment;

namespace RetailECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CheckoutFacade _checkoutFacade;
        private readonly MyDbContext _context;
        private readonly IDiscountService _discountService;
        private readonly IQrCodeDetector _qrCodeDetector;

        public CheckoutController(MyDbContext context, IDiscountService discountService, IQrCodeDetector qrCodeDetector)
        {
            _checkoutFacade = new CheckoutFacade();
            _context = context;
            _discountService = discountService;
            _qrCodeDetector = qrCodeDetector;
        }

        private const string CartSessionKey = "ShoppingCart";
        private const string SelectedCartSessionKey = "SelectedCartItems";

        // Resolve the logged-in shopper's id from their auth cookie / session.
        // Login (SignInController) stores the email as the Name claim and in the
        // "UserEmail" session key; we look that email up in the Users table.
        // Falls back to the seeded user 1 if no one is signed in, so checkout
        // still works in a fresh/demo session.
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

                // No signed-in user found - fall back to the seeded demo user.
                return 1;
            }
        }

        private List<CartItem> GetCartItems()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCartItems(List<CartItem> cartItems)
        {
           var cartJson = JsonSerializer.Serialize(cartItems);
           HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        private List<CartItem> GetSelectedCartItems()
        {
            var selectedJson = HttpContext.Session.GetString(SelectedCartSessionKey);

            if (string.IsNullOrEmpty(selectedJson))
            {
                return GetCartItems();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(selectedJson) ?? new List<CartItem>();
        }

        private void SaveSelectedCartItems(List<CartItem> selectedItems)
        {
            var selectedJson = JsonSerializer.Serialize(selectedItems);
            HttpContext.Session.SetString(SelectedCartSessionKey, selectedJson);
        }

        private void LoadCartData()
        {
            // Pull the real cart the shopper built (stored in session by CartController).
            var cartItems = GetSelectedCartItems();
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
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<int> selectedProductIds)
        {
            var cartItems = GetCartItems();

            if (selectedProductIds == null || !selectedProductIds.Any())
            {
                TempData["CartMessage"] = "Please select at least one product to checkout.";
                return RedirectToAction("Index", "Cart");
            }

            var selectedItems = cartItems
                .Where(i => selectedProductIds.Contains(i.ProductId))
                .ToList();

            if (!selectedItems.Any())
            {
                TempData["CartMessage"] = "Selected products are not found in your cart.";
                return RedirectToAction("Index", "Cart");
            }

            SaveSelectedCartItems(selectedItems);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Process(string paymentType, decimal subtotal, string? discountCode, IFormFile? qrProof)
        {
            LoadCartData();

            // Mock QR payment verification: the checkout page scans the QR with
            // the shopper's camera and submits the captured frame; we confirm
            // server-side (ZXing) that a QR code really is in that frame.
            // No QR found -> back to the checkout page so they can rescan.
            if (paymentType?.ToLower() == "qr")
            {
                bool qrDetected = false;
                if (qrProof != null && qrProof.Length > 0)
                {
                    using var stream = qrProof.OpenReadStream();
                    qrDetected = _qrCodeDetector.ContainsQrCode(stream);
                }

                if (!qrDetected)
                {
                    ModelState.AddModelError("qrProof",
                        qrProof == null || qrProof.Length == 0
                            ? "Please scan your payment QR code with your camera to complete the payment."
                            : "We couldn't verify the scanned QR code. Please try scanning again.");
                    return View("Index");
                }
            }

            int userId = CurrentUserId;
            // Temporary reference used while processing payment; the real, saved
            // order gets its own database id below.
            int orderId = new Random().Next(1000, 9999);

            // Apply the chosen discount to the subtotal (before tax). Expired,
            // unknown, or already-used codes are rejected and the full subtotal
            // is charged.
            var discountResult = _discountService.ApplyDiscount(discountCode, subtotal, userId);
            decimal payableSubtotal = discountResult.DiscountedSubtotal;
            decimal tax = Math.Round(payableSubtotal * 0.08m, 2);

            // The shopper's actual cart, used both for payment notifications and
            // for the order line items we persist.
            var cart = GetSelectedCartItems();
            var cartItems = cart.ToDictionary(i => i.Name, i => (object)i.Price);


            var selectedPaymentType = paymentType ?? "cod";

            var checkoutResult = _checkoutFacade.ProcessCheckout(
                selectedPaymentType,
                payableSubtotal,
                orderId,
                userId,
                cartItems
            );

            // Determine the outcome and the status we store on the order.
            // (The badge views expect "Completed" / "Pending" / "Failed".)
            string paymentStatus;
            string orderStatus;

            if (selectedPaymentType?.ToLower() == "cod")
            {
                paymentStatus = "Pending";
                orderStatus = "Pending";
            }
            else if (checkoutResult.IsSuccessful)
            {
                paymentStatus = "Successful";
                orderStatus = "Completed";
            }
            else
            {
                paymentStatus = "Failed";
                orderStatus = "Failed";
            }

            // Persist the order (and its line items) for any order that was
            // actually placed - paid now, or to be paid on delivery. Failed
            // payments are not recorded because the order was never placed.
            int? savedOrderId = null;
            if (paymentStatus != "Failed" && cart.Any())
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = checkoutResult.TotalAmount,
                    Subtotal = discountResult.OriginalSubtotal,
                    Tax = tax,
                    PaymentMethod = FriendlyPaymentName(selectedPaymentType),
                    OrderStatus = orderStatus,
                    OrderItems = cart.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        UnitPrice = c.Price
                    }).ToList()
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
                savedOrderId = order.Id;

                // Order went through, so mark the discount as used by this user
                // (shows as disabled next time they reach checkout).
                if (discountResult.IsApplied && discountResult.Discount != null)
                {
                    _discountService.RecordDiscountUsed(userId, discountResult.Discount.Id);
                }

                // The order is placed - remove only the selected checkout items from cart.
                var fullCart = GetCartItems();

                fullCart = fullCart
                    .Where(item => !cart.Any(selected => selected.ProductId == item.ProductId))
                    .ToList();

                SaveCartItems(fullCart);
                HttpContext.Session.Remove(SelectedCartSessionKey);
            }

            // Use the real saved order id in messaging when we have one.
            var displayOrderId = savedOrderId ?? orderId;
            string notificationMessage = paymentStatus switch
            {
                "Pending" =>
                    $"Payment pending. Order #{displayOrderId} has been placed, but payment will be collected during delivery.",
                "Successful" =>
                    $"Payment successful. Your order #{displayOrderId} has been placed. Transaction ID: {checkoutResult.GetTransactionId()}",
                _ =>
                    $"Payment failed. Order was not placed. Reason: {checkoutResult.Message}"
            };

            var notification = new Notification
            {
                UserId = userId,
                Message = notificationMessage,
                Type = NotificationType.PaymentUpdate,
                OrderId = savedOrderId,
                IsRead = false,
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
            ViewBag.PaymentType = selectedPaymentType;
            ViewBag.PaymentNotification = notificationMessage;

            // Receipt figures, reflecting the discount cut-off.
            ViewBag.Subtotal = discountResult.OriginalSubtotal;
            ViewBag.DiscountApplied = discountResult.IsApplied;
            ViewBag.DiscountAmount = discountResult.DiscountAmount;
            ViewBag.DiscountMessage = discountResult.Message;
            ViewBag.DiscountCode = discountResult.Discount?.DiscountCode;
            ViewBag.Tax = tax;
            ViewBag.Total = checkoutResult.TotalAmount;

            return View("Process");
        }

        // Maps the posted payment type to a human-friendly label stored on the order.
        private static string FriendlyPaymentName(string? paymentType)
        {
            return paymentType?.ToLower() switch
            {
                "card" => "Credit / Debit Card",
                "qr" => "QR Pay",
                "cod" => "Cash on Delivery",
                _ => "Unknown"
            };
        }
    }
}