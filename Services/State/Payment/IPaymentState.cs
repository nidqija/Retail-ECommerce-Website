namespace RetailECommerce.Services.State.Payment;

public interface IPaymentState
{
    void Process(PaymentContext context);

    void Refund(PaymentContext context);

    void Cancel(PaymentContext context);


}