namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public class CancelledOrderStatus : IOrderStatus
{
    public string StatusName => "Cancelled";

    public void ProcessOrder()
    {
        Console.WriteLine("Order is cancelled. Cannot process.");
    }

    public void ShipOrder()
    {
        Console.WriteLine("Order is cancelled. Cannot ship.");
    }

    public void DeliverOrder()
    {
        Console.WriteLine("Order is cancelled. Cannot deliver.");
    }

    public void CancelOrder()
    {
        Console.WriteLine("Order is already cancelled.");
    }
}