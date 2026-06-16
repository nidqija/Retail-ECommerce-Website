using RetailECommerce.Models;

namespace RetailECommerce.Services.Observers;

public class NotificationEventData
{
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public int? ProductId { get; set; }
    public string? Tab { get; set; }
    public int? OrderId { get; set; }
    public int? TargetUserId { get; set; }
}