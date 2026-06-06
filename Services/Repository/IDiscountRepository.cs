namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;




public interface IDiscountRepository
{
    IEnumerable<Discount> GetAllDiscounts();

    // Only discounts that are currently within their start/end date window.
    IEnumerable<Discount> GetActiveDiscounts();

    Discount GetDiscountById(int id);

    // Returns null when the code does not exist (case-insensitive lookup).
    Discount? GetDiscountByCode(string code);
    void AddDiscount(Discount discount);
    void UpdateDiscount(Discount discount);
    void DeleteDiscount(int id);

    void DeactivateDiscount(int id);

    void ActivateDiscount(int id);
}