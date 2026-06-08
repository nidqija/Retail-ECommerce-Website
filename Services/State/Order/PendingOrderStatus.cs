namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public class PendingOrderStatus : IOrderStatus
{
    public string StatusName => "Pending";

    public void ProcessOrder()
    {
        Console.WriteLine("Processing pending order.");
    }

    public void ShipOrder()
    {
        Console.WriteLine("Order is pending. Cannot ship.");
    }

    public void DeliverOrder()
    {
        Console.WriteLine("Order is pending. Cannot deliver.");
    }

    public void CancelOrder()
    {
        Console.WriteLine("Cancelling pending order.");
    }
}