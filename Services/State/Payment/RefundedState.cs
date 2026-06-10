namespace RetailECommerce.Services.State.Payment;



public class RefundedState : IPaymentState
{
    public void Process(PaymentContext context)
    {
        Console.WriteLine("Processing payment in refunded state...");
        // Transition to the next state, e.g., CompletedState
    }

    public void Refund(PaymentContext context)
    {
        Console.WriteLine("Cannot refund a payment in refunded state.");
    }

    public void Cancel(PaymentContext context)
    {
        Console.WriteLine("Cancelling payment in refunded state...");
        // Transition to the next state, e.g., CancelledState
    }
}