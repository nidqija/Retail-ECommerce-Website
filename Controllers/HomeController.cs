using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
namespace RetailECommerce.Controllers;


public class HomeController : Controller
{
    private IProductRepository _productRepository;

    public HomeController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    // this method returns the home page view using the factory to get the correct handler
    public IActionResult Index()
    {
        
        // parse the products data from db and pass it to the view using viewbag
        var products = _productRepository.GetAllProducts();
        ViewBag.Products = products;

        PageCreator pageCreator = new HomePageCreator();
        return pageCreator.RenderPage(this);


    }


     
}