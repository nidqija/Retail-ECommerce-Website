using System;

namespace RetailECommerce.Services.Strategy.Payment
{
    /// <summary>
    /// Encapsulates the result of a payment execution.
    /// Replaces simple bool return with rich result information.
    /// </summary>
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Factory method for successful payment result.
        /// </summary>
        public static PaymentResult Success(decimal amount, string paymentMethod, string transactionId = "")
        {
            return new PaymentResult
            {
                IsSuccessful = true,
                Amount = amount,
                PaymentMethod = paymentMethod,
                TransactionId = transactionId ?? Guid.NewGuid().ToString(),
                ExecutedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Factory method for failed payment result.
        /// </summary>
        public static PaymentResult Failure(decimal amount, string paymentMethod, string errorMessage = "")
        {
            return new PaymentResult
            {
                IsSuccessful = false,
                Amount = amount,
                PaymentMethod = paymentMethod,
                ErrorMessage = errorMessage,
                ExecutedAt = DateTime.UtcNow
            };
        }
    }
}
