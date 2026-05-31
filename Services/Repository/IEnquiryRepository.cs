namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;

public interface IEnquiryRepository
{
    IEnumerable<Enquiry> GetAllEnquiries();
    Enquiry GetEnquiryById(int id);
    void AddEnquiry(Enquiry enquiry);
    void UpdateEnquiry(Enquiry enquiry);
    void DeleteEnquiry(int id);
}