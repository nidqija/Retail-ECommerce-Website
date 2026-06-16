namespace RetailECommerce.Services.Observers;

public interface INotificationObserver
{
    void Update(NotificationEventData eventData);
}