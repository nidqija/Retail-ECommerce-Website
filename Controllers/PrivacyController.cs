using Microsoft.AspNetCore.Mvc;

namespace RetailECommerce.Controllers;

[Route("privacy-policy")] 
public class PrivacyController : Controller
{
    [Route("")] 
    public IActionResult Index()
    {
        return View();
    }
}