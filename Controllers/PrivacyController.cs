using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;

public class PrivacyController : Controller
{
    // this method returns the privacy page view using the factory to get the correct handler
    public IActionResult Index() 
    {
        // this index method returns the privacy page view using the factory to get the correct handler
        PageCreator pageCreator = new PrivacyPageCreator();
        return pageCreator.RenderPage(this);
    }
}