namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public class PendingOrderStatus : IOrderStatus
{
    public string StatusName => "Pending";
    


    public bool CanTransitionToNextStatus(string targetStatusName)
    {
        return targetStatusName == "Processing" || targetStatusName == "Cancelled";
    }

    
}