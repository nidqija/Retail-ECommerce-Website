namespace RetailECommerce.Services.Observers;

public interface INotificationSubject
{
    void Attach(INotificationObserver observer);
    void Detach(INotificationObserver observer);
    void Clear();
    void Notify(NotificationEventData eventData);
}