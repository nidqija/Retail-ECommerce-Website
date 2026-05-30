using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CardPayment : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(decimal amount)
        {
            // Simulate calling a payment gateway like Stripe or PayPal
            Console.WriteLine($"[Gateway] Attempting to charge Card for {amount:C}");
            
            // For simulation: we'll pretend the payment always succeeds unless the amount is 0
            if (amount <= 0)
            {
                Console.WriteLine("[Gateway] Error: Invalid amount.");
                return PaymentResult.Failure(amount, "Card", "Invalid amount provided.");
            }

            Console.WriteLine("[Gateway] Transaction Approved.");
            return PaymentResult.Success(amount, "Card", $"TXN-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
        }
    }
}