//using Salamidis;
//using Deeznuts;

namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;


public class AdminControllerFacade : Controller
{
    public IActionResult Index()
    {
        PageCreator pageCreator = new AdminHomePageCreator();
        return pageCreator.RenderPage(this);
    }


}