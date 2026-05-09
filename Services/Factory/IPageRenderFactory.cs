using Microsoft.AspNetCore.Mvc;
namespace RetailECommerce.Services.Factory;


//
// Factory Interface: Defines the method to get the appropriate page handler
public interface IPageRenderFactory
{
    IPageHandler GetHandler(string pageName , Controller controller);
}

   


