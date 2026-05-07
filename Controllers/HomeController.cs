using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services;

namespace RetailECommerce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPageRenderFactory _pageRenderFactory;
    
    // depends on IPageRenderFactory to render views
    public HomeController(ILogger<HomeController> logger, IPageRenderFactory pageRenderFactory)
    {
        _logger = logger;
        _pageRenderFactory = pageRenderFactory;
    }

   // register pages in the factory and render them through the factory
   // concrete product 
    public IActionResult Index()
    {
        return _pageRenderFactory.RenderPage("Index");
    }

    // privacy is registered because the navbar link points to Home/Privacy, 
    // but the actual view is in Privacy/Index, so we need to register it in the factory
    public IActionResult Privacy()
    {
        return _pageRenderFactory.RenderPage("Privacy");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return _pageRenderFactory.RenderPage("Error");
    }
}