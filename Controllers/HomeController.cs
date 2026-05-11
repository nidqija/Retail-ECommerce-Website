using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;


public class HomeController : Controller
{
    // this method returns the home page view using the factory to get the correct handler
    public IActionResult Index()
    {
        PageCreator pageCreator = new HomePageCreator();
        return pageCreator.RenderPage(this);
    }
}