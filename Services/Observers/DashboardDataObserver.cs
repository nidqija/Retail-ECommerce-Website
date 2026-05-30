using System;

namespace RetailECommerce.Services.Observers
{
    /// <summary>
    /// Observer that updates dashboard metrics when payment succeeds.
    /// </summary>
    public class DashboardDataObserver : IPaymentObserver
    {
        public void OnPaymentSuccess(PaymentEventData eventData)
        {
            // Simulate updating dashboard data store
            Console.WriteLine($"\n[Dashboard] Recording successful transaction");
            Console.WriteLine($"  Order ID: {eventData.OrderId}");
            Console.WriteLine($"  Revenue: {eventData.Amount:C}");
            Console.WriteLine($"  Payment Method: {eventData.PaymentMethod}");
            Console.WriteLine($"  Status: SUCCESS");
            
            // In a real implementation, this would update database or analytics service
            // e.g., await _dashboardService.RecordTransaction(eventData);
        }

        public void OnPaymentFailure(PaymentEventData eventData)
        {
            // Simulate recording failed payment for dashboard analytics
            Console.WriteLine($"\n[Dashboard] Recording failed transaction");
            Console.WriteLine($"  Order ID: {eventData.OrderId}");
            Console.WriteLine($"  Attempted Amount: {eventData.Amount:C}");
            Console.WriteLine($"  Payment Method: {eventData.PaymentMethod}");
            Console.WriteLine($"  Status: FAILED");
            Console.WriteLine($"  Reason: {eventData.ErrorMessage}");
        }
    }
}
