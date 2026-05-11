using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;

namespace RetailECommerce.Controllers;

// This controller handles the sign-up page requests
public class SignUpController : Controller
{
    private readonly IUserService _userService;

    // constructor injection of factory and user service to handle page rendering and user registration logic
    public SignUpController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index() 
    {
        PageCreator pageCreator = new SignUpPageCreator();
        return pageCreator.RenderPage(this);
    }

    // This method handles the POST request for user registration
    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            bool isRegistered = await _userService.RegisterUserAsync(user);
            if (isRegistered)
            {
            
                Console.WriteLine("User registered successfully.");
                return RedirectToAction("Index", "SignIn");
                 }
            else {
                Console.WriteLine("Registration failed. Email already exists.");
                ModelState.AddModelError("", "Email already exists. Please use a different email.");
                return View("Index", user);
                }
        }
        return View("Index", user);
    }


    
}