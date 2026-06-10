namespace RetailECommerce.Services.State.Payment;




public class SuccessState : IPaymentState
{
    public void Process(PaymentContext context)
    {
        Console.WriteLine("Payment is already successful. No further processing needed.");
    }

    public void Refund(PaymentContext context)
    {
        Console.WriteLine("Refunding payment in success state...");
        // Transition to the next state, e.g., RefundedState
    }

    public void Cancel(PaymentContext context)
    {
        Console.WriteLine("Cannot cancel a payment that is already successful.");
    }
}