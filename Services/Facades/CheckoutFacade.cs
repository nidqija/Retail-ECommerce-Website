using System;
using System.Collections.Generic;
using RetailECommerce.Services.Observers;
using RetailECommerce.Services.Strategy.Payment;

namespace RetailECommerce.Services.Facades
{
    /// <summary>
    /// Checkout Facade - Simplifies the complex checkout orchestration.
    /// Wraps the entire checkout sequence into a single unified interface.
    /// Keeps the controller lean and focused on HTTP concerns only.
    /// </summary>
    public class CheckoutFacade
    {
        private readonly CheckoutContext _checkoutContext;
        private readonly IPaymentObserver _dashboardObserver;
        private readonly IPaymentObserver _receiptObserver;

        public CheckoutFacade()
        {
            _checkoutContext = new CheckoutContext();
            _dashboardObserver = new DashboardDataObserver();
            _receiptObserver = new ReceiptNotificationObserver();

            // Subscribe observers automatically
            _checkoutContext.Subscribe(_dashboardObserver);
            _checkoutContext.Subscribe(_receiptObserver);
        }

        /// <summary>
        /// Simplified checkout method - all logic is orchestrated here.
        /// The controller just needs to call this method.
        /// </summary>
        /// <param name="paymentMethod">Payment method type (card, qr, cod)</param>
        /// <param name="subtotal">Subtotal amount before tax</param>
        /// <param name="orderId">The order ID for state tracking</param>
        /// <param name="userId">The user ID for notifications</param>
        /// <param name="cartItems">Optional cart items for notifications</param>
        /// <returns>Checkout result containing payment result and order state</returns>
        public CheckoutResult ProcessCheckout(
            string paymentMethod,
            decimal subtotal,
            int orderId,
            int userId,
            Dictionary<string, object>? cartItems = null)
        {
            try
            {
                // Validate inputs
                ValidateCheckoutInputs(paymentMethod, subtotal);

                // Select payment strategy
                var strategy = SelectPaymentStrategy(paymentMethod);
                _checkoutContext.SetPaymentStrategy(strategy);

                // Execute payment with tax calculation and observer notifications
                var paymentResult = _checkoutContext.ExecutePayment(
                    subtotal,
                    orderId,
                    userId,
                    cartItems);

                // Get final order state
                var orderState = _checkoutContext.GetOrderState();

                return new CheckoutResult
                {
                    IsSuccessful = paymentResult.IsSuccessful,
                    PaymentResult = paymentResult,
                    OrderState = orderState,
                    TotalAmount = paymentResult.Amount,
                    Message = paymentResult.IsSuccessful
                        ? $"Payment of {paymentResult.Amount:C} via {paymentMethod.ToUpper()} processed successfully!"
                        : $"Payment failed: {paymentResult.ErrorMessage}"
                };
            }
            catch (Exception ex)
            {
                return new CheckoutResult
                {
                    IsSuccessful = false,
                    Message = ex.Message,
                    PaymentResult = null,
                    OrderState = null
                };
            }
        }

        /// <summary>
        /// Add custom observers for additional payment event handling.
        /// </summary>
        public void AddObserver(IPaymentObserver observer)
        {
            _checkoutContext.Subscribe(observer);
        }

        /// <summary>
        /// Set a custom tax calculator.
        /// </summary>
        public void SetTaxCalculator(ITaxCalculator taxCalculator)
        {
            _checkoutContext.SetTaxCalculator(taxCalculator);
        }

        /// <summary>
        /// Calculate the total including tax (useful for previews).
        /// </summary>
        public decimal CalculateTotal(decimal subtotal)
        {
            return _checkoutContext.CalculateTotal(subtotal);
        }

        /// <summary>
        /// Validate checkout inputs.
        /// </summary>
        private void ValidateCheckoutInputs(string paymentMethod, decimal subtotal)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method cannot be empty.");

            if (subtotal <= 0)
                throw new ArgumentException("Subtotal must be greater than zero.");
        }

        /// <summary>
        /// Factory method to select appropriate payment strategy.
        /// </summary>
        private IPaymentStrategy SelectPaymentStrategy(string paymentMethod)
        {
            return paymentMethod.ToLower() switch
            {
                "card" => new CardPayment(),
                "qr" => new QRPayment(),
                "cod" => new CashOnDelivery(),
                _ => throw new ArgumentException($"Unknown payment method: {paymentMethod}")
            };
        }
    }

    /// <summary>
    /// Result object for checkout operations.
    /// Provides a complete summary of the checkout process.
    /// </summary>
    public class CheckoutResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaymentResult? PaymentResult { get; set; }
        public OrderState? OrderState { get; set; }
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Get a user-friendly message for the view.
        /// </summary>
        public string GetDisplayMessage()
        {
            return Message;
        }

        /// <summary>
        /// Get the transaction ID if successful.
        /// </summary>
        public string? GetTransactionId()
        {
            return PaymentResult?.TransactionId;
        }

        /// <summary>
        /// Get the order status as string.
        /// </summary>
        public string GetOrderStatus()
        {
            if (OrderState == null)
                return "Unknown";

            return OrderState.CurrentStatus switch
            {
                OrderState.Status.Pending => "Pending",
                OrderState.Status.PaymentSuccessful => "Successful",
                OrderState.Status.PaymentFailed => $"Failed - {OrderState.FailureReason}",
                OrderState.Status.Cancelled => "Cancelled",
                _ => "Unknown"
            };
        }
    }
}
