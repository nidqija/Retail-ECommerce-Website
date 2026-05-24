using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CardPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Simulate calling a payment gateway like Stripe or PayPal
            Console.WriteLine($"[Gateway] Attempting to charge Card for {amount:C}");
            
            // For simulation: we'll pretend the payment always succeeds unless the amount is 0
            if (amount <= 0)
            {
                Console.WriteLine("[Gateway] Error: Invalid amount.");
                return false; 
            }

            Console.WriteLine("[Gateway] Transaction Approved.");
            return true; 
        }
    }
}