namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;



public class DiscountController : Controller
{

    private IDiscountRepository _discountRepository;


    public DiscountController(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    
    public IActionResult Index()
    {
        var discounts = _discountRepository.GetAllDiscounts().ToList();
        ViewBag.Discounts = discounts;
        
        PageCreator creator = new DiscountsPageCreator();
        return creator.RenderPage(this);
    }


    [HttpPost]
    public IActionResult DeactivateDiscount(int id)
    {
        try
        {
            var discount = _discountRepository.GetDiscountById(id);
            if (discount == null)
            {
                return NotFound("Discount not found");
            }

            _discountRepository.DeactivateDiscount(id);
            Console.WriteLine($"Discount with ID {id} deactivated successfully.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deactivating the discount." + ex.Message);
        }
    }


    [HttpPost]
    public IActionResult ActivateDiscount(int id)
    {
        try
        {
            var discount = _discountRepository.GetDiscountById(id);
            if (discount == null)
            {
                return NotFound("Discount not found");
            }

            _discountRepository.ActivateDiscount(id);
            Console.WriteLine($"Discount with ID {id} activated successfully.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while activating the discount." + ex.Message );
        }
    }



    [HttpPost]
    public IActionResult AddDiscount(Discount discount)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _discountRepository.AddDiscount(discount);
                Console.WriteLine($"Discount with code {discount.DiscountCode} added successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Invalid discount data.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the discount: " + ex.Message);
        }
    }


    [HttpPost]
    public IActionResult DeleteDiscount(int id)
    {
        try
        {
            var discount = _discountRepository.GetDiscountById(id);
            if (discount == null)
            {
                return NotFound("Discount not found");
            }

            _discountRepository.DeleteDiscount(id);
            Console.WriteLine($"Discount with ID {id} deleted successfully.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the discount. , ex: " + ex.Message);
        }
    }


    [HttpPost]
    public IActionResult UpdateDiscount(Discount discount)
    {
        var existingDiscount = _discountRepository.GetDiscountById(discount.Id);


        if (existingDiscount == null)
        {
            return NotFound("Discount not found");
        }

        try
        {
            _discountRepository.UpdateDiscount(discount);
            Console.WriteLine($"Discount with ID {discount.Id} updated successfully.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the discount. , ex: " + ex.Message);
        }
    }


}