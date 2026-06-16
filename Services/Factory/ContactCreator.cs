namespace RetailECommerce.Services.Factory;
using Microsoft.AspNetCore.Mvc;


// this is concrete creator class that implements the factory method to create specific page handlers
// this is the second step in the factory pattern, 
//where we create concrete creator classes that implement the factory method to create specific page handlers
public class HomePageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new IndexPageHandler();
    }
}

public class PrivacyPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new PrivacyPageHandler();
    }
}


public class SignUpPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new SignUpPageHandler();
    }
}


public class SignInPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new SignInPageHandler();
    }
}

public class ReportPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new ReportPageHandler();
    }
}

public class AdminHomePageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminHomePageHandler();
    }
}

// ── Customer Storefront Creators ──────────────────────────────────────────

public class ProductsIndexPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new ProductsIndexPageHandler();
    }
}

public class ProductsDetailsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new ProductsDetailsPageHandler();
    }
}

public class CartPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new CartPageHandler();
    }
}

public class CheckoutPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new CheckoutPageHandler();
    }
}

public class AccountOrdersPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AccountOrdersPageHandler();
    }
}

public class AccountOrderDetailsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AccountOrderDetailsPageHandler();
    }
}

// ── Admin Panel Creators ───────────────────────────────────────────────────

public class AdminProductsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminProductsPageHandler();
    }
}

public class AdminCreateProductPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminCreateProductPageHandler();
    }
}

public class AdminEditProductPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminEditProductPageHandler();
    }
}

public class AdminOrdersPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminOrdersPageHandler();
    }
}

public class AdminOrderDetailsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AdminOrderDetailsPageHandler();
    }
}

public class EditProfilePageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new EditProfilePageHandler();
    }
}

public class EnquiriesPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new EnquiriesPageHandler();
    }
}

public class ReviewsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new ReviewsPageHandler();
    }
}

public class DiscountsPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new DiscountsPageHandler();
    }
}


public class NewPasswordPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new NewPasswordPageHandler();
    }
}