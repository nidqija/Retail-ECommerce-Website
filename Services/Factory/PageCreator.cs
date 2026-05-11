namespace RetailECommerce.Services.Factory;
using Microsoft.AspNetCore.Mvc;

public abstract class PageCreator
{
    public abstract IPageHandler CreatePageHandler();

    public IActionResult RenderPage(Controller controller)
    {
        IPageHandler handler = CreatePageHandler();
        return handler.Render(controller);
    }

}