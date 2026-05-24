using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class QRPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Simulate verifying a generated QR code payment
            Console.WriteLine($"[Gateway] Verifying QR Code transaction for {amount:C}");
            
            if (amount <= 0) return false;

            Console.WriteLine("[Gateway] QR Transaction Verified.");
            return true;
        }
    }
}