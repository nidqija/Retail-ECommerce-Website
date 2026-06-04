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

// ── Customer Storefront Handlers ─────────────────────────────────────────────

public class ProductsIndexPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Products/Index.cshtml");
    }
}

public class ProductsDetailsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Products/Details.cshtml");
    }
}

public class CartPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Cart/Index.cshtml");
    }
}

public class CheckoutPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Checkout/Index.cshtml");
    }
}

public class AccountOrdersPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Account/Orders.cshtml");
    }
}

public class AccountOrderDetailsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Account/OrderDetail.cshtml");
    }
}

// ── Admin Panel Handlers ──────────────────────────────────────────────────────

public class AdminProductsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Products.cshtml");
    }
}

public class AdminCreateProductPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/CreateProduct.cshtml");
    }
}

public class AdminEditProductPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/EditProduct.cshtml");
    }
}

public class AdminOrdersPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Orders.cshtml");
    }
}

public class AdminOrderDetailsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/OrderDetails.cshtml");
    }
}

public class EditProfilePageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Account/EditAccount.cshtml");
    }
}

public class EnquiriesPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Enquiries.cshtml");
    }
}


public class ReviewsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Review.cshtml");
    }
}


public class DiscountsPageHandler : IPageHandler
{
    public IActionResult Render(Controller controller)
    {
        return controller.View("~/Views/Admin/Discounts.cshtml");
    }
}