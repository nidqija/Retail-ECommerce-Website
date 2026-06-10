namespace RetailECommerce.Services.State.Payment;
using RetailECommerce.Services.Strategy.Payment;
using System;



public class PendingState : IPaymentState
{
    public void Process(PaymentContext context)
    {
        Console.WriteLine("Processing payment in pending state...");
        // Transition to the next state, e.g., CompletedState

        // use the strategy to process the payment
        PaymentResult result = context.Strategy.ProcessPayment(context.Amount);
        context.Result = result;

       // transition to success or failed state based on the result
        if(!result.IsSuccessful)
        {
            context.TransitionTo(new FailedState());
            return;
        }
        else
        {
            if (result.PaymentMethod == "QR")
            {
                Console.WriteLine("Payment successful with QR code. Transitioning to SuccessState.");
                bool userScannedCode = QRCodeBuffer();

                if(userScannedCode)
                {
                    Console.WriteLine("User scanned QR code successfully. Transitioning to SuccessState.");
                    context.TransitionTo(new SuccessState());
                }
                else
                {
                    Console.WriteLine("User failed to scan QR code. Transitioning to FailedState.");
                    context.TransitionTo(new FailedState());
                }

            }
        }
    }


    private bool QRCodeBuffer()
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine("Waiting for user to scan QR code...");
            System.Threading.Thread.Sleep(1000); // Simulate waiting time
        }
        return true;
    }

    public void Refund(PaymentContext context)
    {
        Console.WriteLine("Cannot refund a payment in pending state.");
    }

  
}