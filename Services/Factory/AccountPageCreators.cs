namespace RetailECommerce.Services.Factory;

// Factory for Account Orders Page
public class AccountOrderPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AccountOrdersPageHandler();
    }
}

// Factory for Account Order Detail Page
public class AccountOrderDetailPageCreator : PageCreator
{
    public override IPageHandler CreatePageHandler()
    {
        return new AccountOrderDetailsPageHandler();
    }
}
