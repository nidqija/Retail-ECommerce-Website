using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class QRPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Add QR code generation/validation logic here
            Console.WriteLine($"Processing QR Payment of {amount:C}");
            
            return true;
        }
    }
}