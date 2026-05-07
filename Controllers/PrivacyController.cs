using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;

public class PrivacyController : Controller
{
    private readonly IPageRenderFactory _pageRenderFactory;

    public PrivacyController(IPageRenderFactory pageRenderFactory)
    {
        _pageRenderFactory = pageRenderFactory;
    }

    public IActionResult Index()
    {
        return _pageRenderFactory.GetHandler("Index", this).Render(this);
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}