namespace RetailECommerce.Services.Observers;

public class NotificationSubject: INotificationSubject
{
    private readonly List<INotificationObserver> _observers = new();

    public void Attach(INotificationObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(INotificationObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Clear()
    {
        _observers.Clear();
    }

    public void Notify(NotificationEventData eventData)
    {
        foreach (var observer in _observers.ToList())
        {
            observer.Update(eventData);
        }

        Clear();
    }
}