namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;


public class EnquiryController : Controller
{
    public IActionResult Index()
    {
        PageCreator pageCreator = new EnquiriesPageCreator();
        return pageCreator.RenderPage(this);
     }

     
}