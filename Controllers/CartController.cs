namespace RetailECommerce.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;

public class CartController : Controller
{
    private const string CartSessionKey = "ShoppingCart";
    private readonly IProductRepository _productRepository;

    public CartController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        var cartItems = GetCartItems();

        ViewBag.CartItems = cartItems;
        ViewBag.Subtotal = cartItems.Sum(i => i.Price * i.Quantity);

        PageCreator pageCreator = new CartPageCreator();
        return pageCreator.RenderPage(this);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, int quantity = 1)
    {
        if (quantity < 1)
        {
            quantity = 1;
        }

        var product = _productRepository.GetProductById(productId);
        var cartItems = GetCartItems();
        var existingItem = cartItems.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem == null)
        {
            cartItems.Add(new CartItem
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        SaveCartItems(cartItems);
        TempData["CartMessage"] = $"{product.Name} has been added to your cart.";

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCart(int productId, int quantity)
    {
        var cartItems = GetCartItems();
        var item = cartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCartItems(cartItems);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteFromCart(int productId)
    {
        var cartItems = GetCartItems();
        var item = cartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cartItems.Remove(item);
            SaveCartItems(cartItems);
        }

        return RedirectToAction("Index");
    }

    private List<CartItem> GetCartItems()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);

        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<CartItem>();
        }

        return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }

    private void SaveCartItems(List<CartItem> cartItems)
    {
        var cartJson = JsonSerializer.Serialize(cartItems);
        HttpContext.Session.SetString(CartSessionKey, cartJson);
    }
}