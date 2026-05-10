using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;

namespace RetailECommerce.Controllers;

// This controller handles the sign-up page requests
public class SignUpController : Controller
{
    private readonly IPageRenderFactory _factory;

    public SignUpController(IPageRenderFactory factory) => _factory = factory;

    public IActionResult Index() 
    {
        return _factory.GetHandler("signup", this).Render(this);
    }
}