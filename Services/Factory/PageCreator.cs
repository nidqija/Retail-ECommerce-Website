namespace RetailECommerce.Services.Factory;
using Microsoft.AspNetCore.Mvc;

// page creator is responsible for creating page handlers and rendering pages
// this is the first step in the factory pattern, where we define an abstract creator class 
//that declares the factory method (CreatePageHandler) and a method to render the page (RenderPage)
// abstract creator class that defines the factory method and the method to render the page
public abstract class PageCreator
{
    public abstract IPageHandler CreatePageHandler();

    public IActionResult RenderPage(Controller controller)
    {
        IPageHandler handler = CreatePageHandler();
        return handler.Render(controller);
    }

}