namespace RetailECommerce.Services.Observers
{
    /// Subject interface for payment events.
    /// It defines how payment observers subscribe and unsubscribe.

    public interface IPaymentSubject
    {
        void Subscribe(IPaymentObserver observer);
        void Unsubscribe(IPaymentObserver observer);
    }
}