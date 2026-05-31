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
        var enquiry = _context.Enquiries.Find(id);
        
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
        _context.Enquiries.Update(enquiry);
        _context.SaveChanges();
    }

    public void DeleteEnquiry(int id)
    {
        var enquiry = _context.Enquiries.Find(id);
        if (enquiry != null)
        {
            _context.Enquiries.Remove(enquiry);
            _context.SaveChanges();
        }
    }
}