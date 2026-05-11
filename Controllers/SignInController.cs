using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
namespace RetailECommerce.Controllers;

// This controller handles the sign-in page requests
public class SignInController : Controller
{
    private readonly IPageRenderFactory _factory;
    private readonly IUserService _userService;

    public SignInController(IPageRenderFactory factory , IUserService userService) {
            _factory = factory;
            _userService = userService;
    } 

    public IActionResult Index() 
    {
        // One line: Logic and View selection happen in the Factory folder
        return _factory.GetHandler("signin", this).Render(this);
    }


    [HttpPost]
    public async Task<IActionResult> Authenticate(string email , string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return View("Index");
        } else if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid input. Please check your email and password.");
            return View("Index");
        }

        var user = await _userService.AuthenticateUserAsync(email, password);
        if (user != null)
        {
            
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            Console.WriteLine("User authenticated successfully.");
            return RedirectToAction("Index", "Home");
        }
        else
        {
            Console.WriteLine("Authentication failed. Invalid email or password.");
            ModelState.AddModelError("", "Invalid email or password. Please try again.");
            return View("Index");
        }
    }

    

}