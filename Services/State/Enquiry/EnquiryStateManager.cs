namespace RetailECommerce.Services.State.Enquiry; 
using RetailECommerce.Models;


// 3. CONTEXT CLASS
// this class manages the current state of an enquiry and delegates actions to the current state object
// this context class can avoid the enquiry state to have race conditions
// as it only takes one state object at a time and ensures that the state transitions are handled correctly

public class EnquiryStateManager
{


    public Enquiry Enquiry { get; private set; }

    public IEnquireState CurrentState { get; private set; }


    public EnquiryStateManager(Enquiry enquiry)
    {
        Enquiry = enquiry;
        
        CurrentState = enquiry.Status switch
        {
            "Pending" => new PendingState(),
            "Replied" => new RepliedState(),
            "Closed" => new ClosedState(),
            _ => throw new InvalidOperationException("Invalid enquiry status")
        };
    }


    public void TransitionToState(IEnquireState newState)
    {
        CurrentState = newState;
        Enquiry.Status = newState.StatusName;
    }


    public void SubmitResponse(string replyMessage)
    {
        CurrentState.SubmitResponse(this, replyMessage);
    }

    public void CloseEnquiry()
    {
        CurrentState.CloseEnquiry(this);
    }

    

    
}