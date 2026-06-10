namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class ShippedOrderStatus : IOrderStatus
{
    public string StatusName => "Shipped";

    public bool CanTransitionToNextStatus(string targetStatusName)
    {
        return targetStatusName == "Completed";
    }
}