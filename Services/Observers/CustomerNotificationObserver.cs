using RetailECommerce.Models;

namespace RetailECommerce.Services.Observers;

public class CustomerNotificationObserver : INotificationObserver
{
    private readonly MyDbContext _context;

    public CustomerNotificationObserver(MyDbContext context)
    {
        _context = context;
    }

    public void Update(NotificationEventData eventData)
    {
        if (!eventData.TargetUserId.HasValue)
        {
            return;
        }

        _context.Notifications.Add(new Notification
        {
            UserId = eventData.TargetUserId.Value,
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