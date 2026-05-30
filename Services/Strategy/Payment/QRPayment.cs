using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class QRPayment : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(decimal amount)
        {
            // Simulate verifying a generated QR code payment
            Console.WriteLine($"[Gateway] Verifying QR Code transaction for {amount:C}");
            
            if (amount <= 0)
            {
                return PaymentResult.Failure(amount, "QR", "Invalid amount provided.");
            }

            Console.WriteLine("[Gateway] QR Transaction Verified.");
            return PaymentResult.Success(amount, "QR", $"QR-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
        }
    }
}