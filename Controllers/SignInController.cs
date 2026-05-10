using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;

// This controller handles the sign-in page requests
public class SignInController : Controller
{
    private readonly IPageRenderFactory _factory;

    public SignInController(IPageRenderFactory factory) => _factory = factory;

    public IActionResult Index() 
    {
        // One line: Logic and View selection happen in the Factory folder
        return _factory.GetHandler("signin", this).Render(this);
    }
}