using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace RetailECommerce.Controllers;

// This controller handles the sign-in page requests
public class SignInController : Controller
{
    private readonly IUserService _userService;

    public SignInController(IUserService userService) {
            _userService = userService;
    } 

    public IActionResult Index(string ? ReturnUrl = null) 
    {

        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            ViewBag.ShowError = true;
            ViewBag.ErrorMessage = "You must be signed in to access that page.";
        } else {
            ViewBag.ShowError = false;
        }

        ViewBag.ReturnUrl = ReturnUrl; 

        // One line: Logic and View selection happen in the Factory folder
        PageCreator pageCreator = new SignInPageCreator();
        return pageCreator.RenderPage(this);
    }


    // only use task and async when you have to do something that takes time, 
    // like database access or calling an external API
    [HttpPost]
    public async Task<IActionResult> Authenticate(string email , string password , string ? ReturnUrl = null)
    {
  

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return Index(ReturnUrl); // FIX: Routes back through Index logic to preserve Factory UI setup safely
        } else if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid input. Please check your email and password.");
            return Index(ReturnUrl); // FIX: Routes back through Index logic to preserve Factory UI setup safely
        }

        var user = await _userService.AuthenticateUserAsync(email, password);
        if (user != null)
        {
            // store user information in session to persist data across requests
            //  and maintain user state
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("FullName", user.FullName);

            // create claims for the authenticated user, which will be used to create a claims identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("FullName", user.FullName)
            };

            // create a claims identity and sign in the user using cookie authentication
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // async method to sign in the user and create an authentication cookie that will be sent to the client
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            // Fallback tracking defaults if no prior target was registered
            if (user.Role == UserRole.Vendor)
            {
                return RedirectToAction("Index", "Admin");
            }

            Console.WriteLine("User authenticated successfully.");
            Console.WriteLine("User Password: " + user.Password);
            Console.WriteLine("User Full Name: " + user.FullName);

            
            return RedirectToAction("Index", "Home");
        }
        else
        {
            Console.WriteLine("Authentication failed. Invalid email or password.");
            ModelState.AddModelError("", "Invalid email or password. Please try again.");
            return Index(ReturnUrl); // FIX: Routes back through Index logic to preserve Factory UI setup safely}
    }
    }


    

    

}