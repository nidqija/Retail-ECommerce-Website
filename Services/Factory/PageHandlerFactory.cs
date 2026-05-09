using Microsoft.AspNetCore.Mvc;
namespace RetailECommerce.Services.Factory;


// page factory implementation for rendering pages
public class PageHandlerFactory : IPageRenderFactory
{
    public IPageHandler GetHandler(string pageType , Controller controller)
    {
        return pageType.ToLowerInvariant() switch
        {
            "index" => new IndexPageHandler(),
            "privacy" => new PrivacyPageHandler(),
            _ => new IndexPageHandler()
        };
    }
}