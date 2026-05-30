using System;
using System.Collections.Generic;
using RetailECommerce.Services.Observers;

namespace RetailECommerce.Services.Strategy.Payment
{
    /// <summary>
    /// Manages order state transitions (Pending -> Success/Failed).
    /// Encapsulates the order state machine logic.
    /// </summary>
    public class OrderState
    {
        public enum Status
        {
            Pending,
            PaymentSuccessful,
            PaymentFailed,
            Cancelled
        }

        public int OrderId { get; set; }
        public Status CurrentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string? FailureReason { get; set; }

        public OrderState(int orderId)
        {
            OrderId = orderId;
            CurrentStatus = Status.Pending;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void TransitionToPaymentSuccess()
        {
            if (CurrentStatus == Status.Pending)
            {
                CurrentStatus = Status.PaymentSuccessful;
                LastUpdatedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException($"Cannot transition from {CurrentStatus} to PaymentSuccessful");
            }
        }

        public void TransitionToPaymentFailure(string reason)
        {
            if (CurrentStatus == Status.Pending)
            {
                CurrentStatus = Status.PaymentFailed;
                FailureReason = reason;
                LastUpdatedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException($"Cannot transition from {CurrentStatus} to PaymentFailed");
            }
        }
    }

    /// <summary>
    /// Enhanced checkout context with observer support and order state management.
    /// </summary>
    public class CheckoutContext
    {
        private IPaymentStrategy? _paymentStrategy;
        private ITaxCalculator? _taxCalculator;
        private readonly List<IPaymentObserver> _observers = new();
        private OrderState? _orderState;

        public CheckoutContext()
        {
            _taxCalculator = new StandardTaxCalculator(0.08m); // Default 8% tax
        }

        /// <summary>
        /// Set the payment strategy to use.
        /// </summary>
        public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        /// <summary>
        /// Set a custom tax calculator.
        /// </summary>
        public void SetTaxCalculator(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        /// <summary>
        /// Subscribe an observer to payment events.
        /// </summary>
        public void Subscribe(IPaymentObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        /// <summary>
        /// Unsubscribe an observer from payment events.
        /// </summary>
        public void Unsubscribe(IPaymentObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Calculate the total amount including tax.
        /// </summary>
        public decimal CalculateTotal(decimal subtotal)
        {
            if (_taxCalculator == null)
                throw new InvalidOperationException("Tax calculator has not been configured.");

            var tax = _taxCalculator.CalculateTax(subtotal);
            return subtotal + tax;
        }

        /// <summary>
        /// Execute the payment and notify all observers.
        /// Returns the payment result and updates order state.
        /// </summary>
        public PaymentResult ExecutePayment(decimal subtotal, int orderId, int userId, Dictionary<string, object>? cartItems = null)
        {
            if (_paymentStrategy == null)
                throw new InvalidOperationException("Payment method has not been selected.");

            _orderState = new OrderState(orderId);

            // Calculate total with tax
            var total = CalculateTotal(subtotal);

            // Execute payment
            var paymentResult = _paymentStrategy.ProcessPayment(total);

            // Update order state
            if (paymentResult.IsSuccessful)
            {
                _orderState.TransitionToPaymentSuccess();
                NotifyPaymentSuccess(orderId, userId, paymentResult, cartItems ?? new());
            }
            else
            {
                _orderState.TransitionToPaymentFailure(paymentResult.ErrorMessage);
                NotifyPaymentFailure(orderId, userId, paymentResult, cartItems ?? new());
            }

            return paymentResult;
        }

        /// <summary>
        /// Notify all observers of successful payment.
        /// </summary>
        private void NotifyPaymentSuccess(int orderId, int userId, PaymentResult result, Dictionary<string, object> cartItems)
        {
            var eventData = new PaymentEventData
            {
                OrderId = orderId,
                UserId = userId,
                Amount = result.Amount,
                PaymentMethod = result.PaymentMethod,
                TransactionId = result.TransactionId,
                ExecutedAt = result.ExecutedAt,
                CartItems = cartItems
            };

            foreach (var observer in _observers)
            {
                observer.OnPaymentSuccess(eventData);
            }
        }

        /// <summary>
        /// Notify all observers of failed payment.
        /// </summary>
        private void NotifyPaymentFailure(int orderId, int userId, PaymentResult result, Dictionary<string, object> cartItems)
        {
            var eventData = new PaymentEventData
            {
                OrderId = orderId,
                UserId = userId,
                Amount = result.Amount,
                PaymentMethod = result.PaymentMethod,
                ExecutedAt = result.ExecutedAt,
                ErrorMessage = result.ErrorMessage,
                CartItems = cartItems
            };

            foreach (var observer in _observers)
            {
                observer.OnPaymentFailure(eventData);
            }
        }

        /// <summary>
        /// Get the current order state.
        /// </summary>
        public OrderState? GetOrderState() => _orderState;
    }
}