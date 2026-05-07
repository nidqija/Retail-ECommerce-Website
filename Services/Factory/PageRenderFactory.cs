using Microsoft.AspNetCore.Mvc;

namespace RetailECommerce.Services;

// concrete factory implementation for rendering pages
public class PageRenderFactory : IPageRenderFactory
{
    private readonly Dictionary<string, Func<IActionResult>> _pageRegistry;

    public PageRenderFactory()
    {
        _pageRegistry = new Dictionary<string, Func<IActionResult>>(StringComparer.OrdinalIgnoreCase);
        RegisterPages();
    }

    private void RegisterPages()
    {
        // Register pages with their view names
        _pageRegistry["Index"] = () => new ViewResult { ViewName = "Index" };
        _pageRegistry["Privacy"] = () => new ViewResult { ViewName = "../Privacy/Privacy" };
        _pageRegistry["Error"] = () => new ViewResult { ViewName = "Error" };
    }

    public IActionResult RenderPage(string pageName)
    {
        if (_pageRegistry.TryGetValue(pageName, out var pageFactory))
        {
            return pageFactory();
        }

        throw new ArgumentException($"Page '{pageName}' not found in registry.");
    }

    public void RegisterPage(string pageName, Func<IActionResult> viewFactory)
    {
        _pageRegistry[pageName] = viewFactory;
    }
}

   


