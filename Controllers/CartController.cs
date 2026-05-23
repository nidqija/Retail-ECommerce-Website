namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;


public class CartController : Controller
{
    // GET: /Cart
    public IActionResult Index()
    {
        // Mock cart items using anonymous data; replace with session/DB cart later
        var cartItems = new[]
        {
            new { ProductId = 1, Name = "Mechanical Keyboard", Price = 89.99m, Quantity = 1 },
        };

        ViewBag.CartItems = cartItems;
        ViewBag.Subtotal  = cartItems.Sum(i => i.Price * i.Quantity);

        PageCreator pageCreator = new CartPageCreator();
        return pageCreator.RenderPage(this);
    }
}
