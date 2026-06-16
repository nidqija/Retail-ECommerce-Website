using RetailECommerce.Models;

namespace RetailECommerce.Services.Observers;

public class AdminNotificationObserver : INotificationObserver
{
    private readonly MyDbContext _context;

    public AdminNotificationObserver(MyDbContext context)
    {
        _context = context;
    }

    public void Update(NotificationEventData eventData)
    {
        var vendors = _context.Users
            .Where(u => u.Role == UserRole.Vendor)
            .ToList();

        foreach (var vendor in vendors)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = vendor.UserId,
                Message = eventData.Message,
                Type = eventData.Type,
                ProductId = eventData.ProductId,
                Tab = eventData.Tab,
                OrderId = eventData.OrderId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}