using System;

namespace RetailECommerce.Services.Observers
{
    /// <summary>
    /// Observer that sends receipt notifications when payment succeeds.
    /// </summary>
    public class ReceiptNotificationObserver : IPaymentObserver
    {
        public void OnPaymentSuccess(PaymentEventData eventData)
        {
            // Simulate sending receipt notification via email/SMS
            Console.WriteLine($"\n[Notification] Receipt sent to User {eventData.UserId}");
            Console.WriteLine($"  Transaction ID: {eventData.TransactionId}");
            Console.WriteLine($"  Amount: {eventData.Amount:C}");
            Console.WriteLine($"  Payment Method: {eventData.PaymentMethod}");
            Console.WriteLine($"  Timestamp: {eventData.ExecutedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Items: {string.Join(", ", eventData.CartItems.Keys)}");
        }

        public void OnPaymentFailure(PaymentEventData eventData)
        {
            // Simulate sending failure notification
            Console.WriteLine($"\n[Notification] Payment Failed Alert sent to User {eventData.UserId}");
            Console.WriteLine($"  Error: {eventData.ErrorMessage}");
            Console.WriteLine($"  Amount Attempted: {eventData.Amount:C}");
        }
    }
}
