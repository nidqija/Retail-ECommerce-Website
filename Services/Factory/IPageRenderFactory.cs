using Microsoft.AspNetCore.Mvc;

namespace RetailECommerce.Services;

// abstract factory interface for rendering pages
public interface IPageRenderFactory
{
    IActionResult RenderPage(string pageName);
}
