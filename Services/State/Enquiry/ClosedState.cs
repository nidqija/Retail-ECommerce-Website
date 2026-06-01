namespace RetailECommerce.Services.State.Enquiry; 
using RetailECommerce.Models;



public class ClosedState : IEnquireState
{
    public string StatusName => "Closed";

    public void SubmitResponse(EnquiryStateManager manager, string replyMessage)
    {
        Console.WriteLine("Cannot submit a response to a closed enquiry. Please reopen the enquiry if you want to submit a new response.");
    }

    public void CloseEnquiry(EnquiryStateManager manager)
    {
        Console.WriteLine("This enquiry is already closed.");
    }
}