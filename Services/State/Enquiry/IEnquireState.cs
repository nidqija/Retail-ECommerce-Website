namespace RetailECommerce.Services.State.Enquiry; 
using RetailECommerce.Models;

// 1. STATE INTERFACE
// define the contract for different states of an enquiry
// define the methods needed to be implemented by each state (Pending, Replied, Closed)
public interface IEnquireState
{
    string StatusName { get; }

    void SubmitResponse(EnquiryStateManager manager, string replyMessage);


    void CloseEnquiry(EnquiryStateManager manager);

    void VendorSubmitResponse(EnquiryStateManager manager, string replyMessage);


}