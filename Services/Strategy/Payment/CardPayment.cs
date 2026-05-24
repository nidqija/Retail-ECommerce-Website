using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CardPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Add real integration logic here (e.g., Stripe, PayPal API)
            Console.WriteLine($"Processing Card Payment of {amount:C}");
            
            // Simulating a successful transaction
            return true; 
        }
    }
}