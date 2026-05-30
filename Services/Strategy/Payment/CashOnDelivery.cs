using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CashOnDelivery : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(decimal amount)
        {
            // Simulate logging the COD order
            Console.WriteLine($"[Logistics] Order flagged for Cash on Delivery. Amount to collect: {amount:C}");
            
            // COD is always "successful" at checkout since payment happens later
            return PaymentResult.Success(amount, "CashOnDelivery", $"COD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
        }
    }
}