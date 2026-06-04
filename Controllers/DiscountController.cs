namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;




public class DiscountController : Controller
{
    
    public IActionResult Index()
    {
        var pageCreator = new DiscountsPageCreator();
        var pageHandler = pageCreator.CreatePageHandler();

        // Render the page using the handler
        return pageHandler.Render(this);
    }




}