using System.Security.Claims;
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

        public IActionResult Index()
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == CurrentUserId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            ViewBag.Notifications = notifications;
            return View();
        }
    }
}