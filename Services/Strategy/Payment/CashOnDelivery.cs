using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    public class CashOnDelivery : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            // Logic for Cash on Delivery (e.g., updating order status to pending payment)
            Console.WriteLine($"Order placed via Cash on Delivery. Collect {amount:C} upon arrival.");
            
            return true;
        }
    }
}