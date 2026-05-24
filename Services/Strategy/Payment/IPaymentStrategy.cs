namespace RetailECommerce.Services.Strategy.Payment
{
    public interface IPaymentStrategy
    {
        // The method that all concrete payment methods must implement
        bool ProcessPayment(decimal amount);
    }
}