using Microsoft.AspNetCore.Mvc;
namespace RetailECommerce.Services.Factory;

// abstract product interface for page handlers
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
        return controller.View("Index");
    }
}

public class SignUpPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("Index");
    }
}

public class SignInPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("Index");
    }
}

public class AdminDashboardPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("Index");
    }
}

public class ReportPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Report.cshtml");
    }
}

public class AdminHomePageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Home.cshtml");
    }
}





