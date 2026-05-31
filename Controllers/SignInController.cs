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

    public IActionResult Index(string ? returnurl = null) 
    {

        if (!string.IsNullOrEmpty(returnurl))
        {
            ViewBag.ShowError = true;
            ViewBag.ErrorMessage = "You must be signed in to access that page.";
        } else {
            ViewBag.ShowError = false;
        }

        ViewBag.ReturnUrl = returnurl; 

        // One line: Logic and View selection happen in the Factory folder
        PageCreator pageCreator = new SignInPageCreator();
        return pageCreator.RenderPage(this);
    }


    // only use task and async when you have to do something that takes time, 
    // like database access or calling an external API
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

            Console.WriteLine("User authenticated successfully.");
            Console.WriteLine("User Password: " + user.Password);
            Console.WriteLine("User Full Name: " + user.FullName);

            if (user.Role == UserRole.Vendor)
            {
                // this calls the Index() method of the AdminController to render the admin dashboard
                // "Admin" is the name of the controller, "Index" is the name of the action method
                return RedirectToAction("Index", "Admin");
            }
             
            
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