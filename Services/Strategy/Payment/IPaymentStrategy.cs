namespace RetailECommerce.Services.Strategy.Payment
{
    /// <summary>
    /// Generic payment execution interface - segregated from tax logic.
    /// Each implementation handles a specific payment method.
    /// </summary>
    public interface IPaymentStrategy
    {
        /// <summary>
        /// Execute payment for the given amount.
        /// Returns a PaymentResult with detailed execution information.
        /// </summary>
        /// <param name="amount">The total amount to charge (including tax)</param>
        /// <returns>PaymentResult containing success/failure details</returns>
        PaymentResult ProcessPayment(decimal amount);
    }
}