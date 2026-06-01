namespace Services.State;
using RetailECommerce.Models;


public interface IEnquireState
{
    // getter for current state name
    string StatusName { get; }

    // method to advance to the next state
    void AdvanceState(EnquiryStateManager manager);

    // method to cancel the enquiry and move to Cancelled state
    void Cancel(EnquiryStateManager manager);


}