using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CashOnDelivery : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Simulate logging the COD order
            Console.WriteLine($"[Logistics] Order flagged for Cash on Delivery. Amount to collect: {amount:C}");
            
            return true; // COD is always "successful" at checkout since payment happens later
        }
    }
}