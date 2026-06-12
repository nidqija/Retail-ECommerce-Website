namespace RetailECommerce.Services.State.Enquiry; 
using RetailECommerce.Models;


// 2. CONCRETE STATES
// implement the IEnquireState interface for each specific state of an enquiry
// this is for pending state where the enquiry is still open and waiting for a response from the vendor

public class PendingState : IEnquireState
{
    public string StatusName => "Pending";

    public void SubmitResponse(EnquiryStateManager manager, string replyMessage)
    {
        manager.Enquiry.ReplyMessage = replyMessage;
        manager.TransitionToState(new RepliedState());

        Console.WriteLine($"Enquiry with ID {manager.Enquiry.EnquiryId} has been replied with this message : {replyMessage}");
    }

    public void CloseEnquiry(EnquiryStateManager manager)
    {
        Console.WriteLine("Cannot close an enquiry that is still pending. Please submit a response first.");
    }

    public void VendorSubmitResponse(EnquiryStateManager manager, string replyMessage)
    {
        manager.Enquiry.ReplyMessage = replyMessage;
        manager.TransitionToState(new RepliedState());

        Console.WriteLine($"Enquiry with ID {manager.Enquiry.EnquiryId} has been replied with this message : {replyMessage}");
    } 
}