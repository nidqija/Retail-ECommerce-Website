namespace RetailECommerce.Services.State.Payment;
using RetailECommerce.Services.Strategy.Payment;





public class PaymentContext
{


    public IPaymentState CurrentState {get ; private set;}
    public IPaymentStrategy Strategy {get; }

    public decimal Amount {get; set;}

    public PaymentResult Result {get; set;}

    

    public PaymentContext(IPaymentStrategy strategy, decimal amount)
    {
        Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        Amount = amount;    
        TransitionTo(new PendingState());
    }


    public void TransitionTo(IPaymentState state)
    {
        Console.WriteLine($"Transitioning to {state.GetType().Name} state.");
        CurrentState = state;
    }


    public void Process()
    {
        CurrentState.Process(this);
    }


    public void Refund()
    {
        CurrentState.Refund(this);
    }



    


    
}