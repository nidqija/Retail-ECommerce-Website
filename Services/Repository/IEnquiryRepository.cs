namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;

public interface IEnquiryRepository
{
    IEnumerable<Enquiry> GetAllEnquiries();
    Enquiry GetEnquiryById(int id);
    void AddEnquiry(Enquiry enquiry);
    void UpdateEnquiry(Enquiry enquiry);

    // for vendor to update the enquiry status and reply message based on vendor message in the view
    void VendorUpdateEnquiry(Enquiry enquiry);
    void DeleteEnquiry(int id);

    void DeleteEnquiryReply(int id);
}