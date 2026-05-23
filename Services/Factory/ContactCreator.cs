namespace RetailECommerce.Services.Factory;
using Microsoft.AspNetCore.Mvc;



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