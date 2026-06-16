using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;

namespace RetailECommerce.Controllers
{
    public class NotificationController : Controller
    {
        private readonly MyDbContext _context;

        public NotificationController(MyDbContext context)
        {
            _context = context;
        }

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

                return 1;
            }
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var notifications = _context.Notifications
                .Where(n => n.UserId == CurrentUserId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            ViewBag.Notifications = notifications;
            return View();
        }

        public IActionResult Open(int id)
        {
            var notification = _context.Notifications
                .FirstOrDefault(n => n.NotificationId == id && n.UserId == CurrentUserId);

            if (notification == null)
            {
                return RedirectToAction("Index");
            }

            notification.IsRead = true;
            _context.SaveChanges();

            var message = notification.Message.ToLower();

            if (notification.Type == NotificationType.NewOrderReceived || message.Contains("new order received"))
            {
                return RedirectToAction("Orders", "Admin");
            }

            if (notification.Type == NotificationType.NewCustomerEnquiry || message.Contains("new customer enquiry"))
            {
                return RedirectToAction("Enquiries", "Admin");
            }

            if (notification.Type == NotificationType.NewCustomerReview || message.Contains("new customer review"))
            {
                return RedirectToAction("Index", "Review");
            }

            if (notification.Type == NotificationType.ProductOutOfStock || message.Contains("out of stock"))
            {
                return RedirectToAction("Products", "Admin");
            }

            if (notification.Type == NotificationType.PaymentUpdate)
            {
                int? orderId = notification.OrderId;

                if (!orderId.HasValue)
                {
                    var match = Regex.Match(notification.Message, @"order #(\d+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        orderId = int.Parse(match.Groups[1].Value);
                    }
                }

                if (orderId.HasValue)
                {
                    return RedirectToAction("OrderDetail", "Account", new { orderId = orderId.Value });
                }
            }

            if (notification.Type == NotificationType.SystemAlert)
            {
                int? productId = notification.ProductId;

                if (!productId.HasValue)
                {
                    var match = Regex.Match(notification.Message, @"product #(\d+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        productId = int.Parse(match.Groups[1].Value);
                    }
                }

                var tab = notification.Tab;

                if (string.IsNullOrEmpty(tab))
                {
                    tab = notification.Message.ToLower().Contains("enquiry") ? "questions" : "feedback";
                }

                if (productId.HasValue)
                {
                    return RedirectToAction("Details", "Products", new
                    {
                        id = productId.Value,
                        tab = tab
                    });
                }
            }

            return RedirectToAction("Index");
        }
    }
}