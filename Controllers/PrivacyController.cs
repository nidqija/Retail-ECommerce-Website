using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;

public class PrivacyController : Controller
{
    private readonly IPageRenderFactory _factory;

    public PrivacyController(IPageRenderFactory factory) => _factory = factory;

    public IActionResult Index() 
    {
        // this index method returns the privacy page view using the factory to get the correct handler
        return _factory.GetHandler("privacy", this).Render(this);
    }
}