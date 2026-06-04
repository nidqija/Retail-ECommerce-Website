namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;




public interface IDiscountRepository
{
    IEnumerable<Discount> GetAllDiscounts();
    Discount GetDiscountById(int id);
    void AddDiscount(Discount discount);
    void UpdateDiscount(Discount discount);
    void DeleteDiscount(int id);

    void DeactivateDiscount(int id);

    void ActivateDiscount(int id);
}