namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class ProcessingOrderStatus : IOrderStatus
{
    public string StatusName => "Processing";


    public bool CanTransitionToNextStatus(string targetStatusName)
    {
        return targetStatusName == "Shipped" || targetStatusName == "Cancelled";
    }


}