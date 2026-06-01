namespace RetailECommerce.Services.State.Enquiry; 
using RetailECommerce.Models;



public class RepliedState : IEnquireState
{
    public string StatusName => "Replied";

    public void SubmitResponse(EnquiryStateManager manager, string replyMessage)
    {
        Console.WriteLine("This enquiry has already been replied to. Please close the enquiry if you want to submit a new response.");
    }
    
    public void CloseEnquiry(EnquiryStateManager manager)
    {
        manager.TransitionToState(new ClosedState());
    }
}