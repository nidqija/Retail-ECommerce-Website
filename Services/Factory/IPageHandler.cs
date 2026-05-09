using Microsoft.AspNetCore.Mvc;
namespace RetailECommerce.Services.Factory;


public interface IPageHandler
{
    // The handler performs the action and returns the result
    IActionResult Render(Controller controller);
}

// Concrete Product: The Home Page logic
public class IndexPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("Index");
    }
}

// Concrete Product: The Privacy Page logic
public class PrivacyPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        // Solving your path issue: Redirecting to the correct view location
        return controller.View("Index");
    }
}