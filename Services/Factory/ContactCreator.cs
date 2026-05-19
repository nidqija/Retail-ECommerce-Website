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