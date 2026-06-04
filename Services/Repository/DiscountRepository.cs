namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;




public class DiscountRepository : IDiscountRepository
{
    private readonly MyDbContext _context;

    public DiscountRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Discount> GetAllDiscounts()
    {
        return _context.Discounts.ToList();
    }

    public Discount GetDiscountById(int id)
    {
        var discount = _context.Discounts.FirstOrDefault(d => d.Id == id);
        if (discount == null)
        {
            throw new Exception($"Discount with ID {id} not found.");
        }
        return discount;
    }

    public void AddDiscount(Discount discount)
    {
        _context.Discounts.Add(discount);
        _context.SaveChanges();
    }

    public void UpdateDiscount(Discount discount)
    {
        var existingDiscount = _context.Discounts.FirstOrDefault(d => d.Id == discount.Id);
        if (existingDiscount != null)
        {
            existingDiscount.Description = discount.Description;
            existingDiscount.DiscountPercentage = discount.DiscountPercentage;
            existingDiscount.EndDate = discount.EndDate;
            existingDiscount.DiscountCode = discount.DiscountCode;
           
            _context.SaveChanges();
            Console.WriteLine($"Discount with ID {discount.Id} updated successfully with this description : {discount.Description}");
        }
    }

    public void DeleteDiscount(int id)
    {
        var discount = _context.Discounts.FirstOrDefault(d => d.Id == id);
        if (discount != null)
        {
            _context.Discounts.Remove(discount);
            _context.SaveChanges();
        }
    }

    public void DeactivateDiscount(int id)
    {
        var discount = _context.Discounts.FirstOrDefault(d => d.Id == id);
        if (discount != null)
        {
            discount.EndDate = DateTime.Now.AddDays(-1); 
            _context.SaveChanges();
        }
    }

    public void ActivateDiscount(int id)
    {
        var discount = _context.Discounts.FirstOrDefault(d => d.Id == id);
        if (discount != null)
        {
            discount.EndDate = DateTime.Now.AddDays(30); 
            _context.SaveChanges();
        }
    }
}