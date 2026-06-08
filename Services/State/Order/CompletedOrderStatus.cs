namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class CompletedOrderStatus : IOrderStatus
{
    public string StatusName => "Completed";

    public void ProcessOrder()
    {
        Console.WriteLine("Order is already completed. Cannot process.");
    }

    public void ShipOrder()
    {
        Console.WriteLine("Order is already completed. Cannot ship.");
    }

    public void DeliverOrder()
    {
        Console.WriteLine("Order is already completed. Cannot deliver.");
    }

    public void CancelOrder()
    {
        Console.WriteLine("Order is already completed. Cannot cancel.");
    }
}