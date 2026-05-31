namespace RetailECommerce.Services.Observers
{
    /// <summary>
    /// Observer interface for payment events.
    /// Implementations subscribe to payment success/failure events.
    /// </summary>
    public interface IPaymentObserver
    {
        /// <summary>
        /// Called when a payment is successfully processed.
        /// </summary>
        void OnPaymentSuccess(PaymentEventData eventData);

        /// <summary>
        /// Called when a payment fails.
        /// </summary>
        void OnPaymentFailure(PaymentEventData eventData);
    }

    /// <summary>
    /// Event data passed to observers.
    /// </summary>
    public class PaymentEventData
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Dictionary<string, object> CartItems { get; set; } = new();
    }
}
