namespace RetailECommerce.Services.State.Payment;


public class FailedState : IPaymentState
{
    public void Process(PaymentContext context)
    {
        Console.WriteLine("Processing payment in failed state...");
        // Transition to the next state, e.g., CompletedState
    }

    public void Refund(PaymentContext context)
    {
        Console.WriteLine("Cannot refund a payment in failed state.");
    }

    
}