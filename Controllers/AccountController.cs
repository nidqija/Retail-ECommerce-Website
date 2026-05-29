namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


public class AccountController : Controller
{

    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    // GET: /Account/Orders  — customer order history
    public IActionResult Orders()
    {
        // Mock past orders; replace with DB query filtered by session UserEmail later
        var orders = new[]
        {
            new { OrderId = 1001, Date = DateTime.Now.AddDays(-30), Total = 419.98m, Status = PaymentStatus.Completed },

            new { OrderId = 1004, Date = DateTime.Now.AddDays(-1),  Total = 658.97m, Status = PaymentStatus.Pending   },
        };

        ViewBag.Orders = orders;

        PageCreator pageCreator = new AccountOrdersPageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Account/Logout
    public async Task<IActionResult> Logout()
    {

        HttpContext.Session.Clear();

        // clear the authentication cookie to log the user out
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }


// ---------------------------------------- Profile Editing ----------------------------------------

    // GET: /Account/EditProfilePage
    [HttpGet]
    public IActionResult EditProfilePage()
    {
        PageCreator pageCreator = new EditProfilePageCreator();
        return pageCreator.RenderPage(this);
    }

    // POST: /Account/EditProfile
    [HttpPost]
    public async Task<IActionResult> EditProfile(string fullName, string email)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            Console.WriteLine("No user email found in session. User may not be logged in.");
            return RedirectToAction("Index", "SignIn");
        }

        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            Console.WriteLine("User not found for email: " + userEmail);
            return RedirectToAction("Index", "SignIn");

        }

        bool isUpdated = _userService.EditUserProfile(user.UserId, fullName, email);
        if (isUpdated)
        {
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("FullName", fullName);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Failed to update profile. Email may already be in use.");
            Console.WriteLine("Failed to update profile for user: " + userEmail);
            PageCreator pageCreator = new EditProfilePageCreator();
            return pageCreator.RenderPage(this);
        }

        
    }

//==========================================================================================================================

}
