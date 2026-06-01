namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;



public class EnquiryRepository : IEnquiryRepository
{
    private readonly MyDbContext _context;

    public EnquiryRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Enquiry> GetAllEnquiries()
    {
        return _context.Enquiries.Include(e => e.User).Include(e => e.Product).ToList();
    }

    public Enquiry GetEnquiryById(int id)
    {
        var enquiry = _context.Enquiries.Include(e => e.User).Include(e => e.Product).FirstOrDefault(e => e.EnquiryId == id);
        
        if (enquiry == null)
        {
            throw new Exception("Enquiry not found");
        }

        return enquiry;
    }

    public void AddEnquiry(Enquiry enquiry)
    {
        _context.Enquiries.Add(enquiry);
        _context.SaveChanges();
    }

    public void UpdateEnquiry(Enquiry enquiry)
    {
        var existingEnquiry = _context.Enquiries.Include(e => e.User).Include(e => e.Product).FirstOrDefault(e => e.EnquiryId == enquiry.EnquiryId);
        if (existingEnquiry != null)
        {
            existingEnquiry.ReplyMessage = enquiry.ReplyMessage;
           
            _context.SaveChanges();
            Console.WriteLine($"Enquiry with ID {enquiry.EnquiryId} updated successfully with this reply message : {enquiry.ReplyMessage}");
        }
    }

    public void DeleteEnquiry(int id)
    {
        var enquiry = _context.Enquiries.Include(e => e.User).Include(e => e.Product).FirstOrDefault(e => e.EnquiryId == id);
        if (enquiry != null)
        {
            _context.Enquiries.Remove(enquiry);
            _context.SaveChanges();
        }
    }
}