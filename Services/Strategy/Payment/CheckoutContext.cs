using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CheckoutContext
    {
        private IPaymentStrategy _paymentStrategy;      

        // Allows the payment method to be set dynamically at runtime
        public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public bool ExecutePayment(decimal amount)
        {
            if (_paymentStrategy == null)
            {
                throw new InvalidOperationException("Payment method has not been selected.");
            }

            return _paymentStrategy.ProcessPayment(amount);
        }
    }
}