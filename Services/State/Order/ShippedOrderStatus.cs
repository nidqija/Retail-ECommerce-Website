namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class ShippedOrderStatus : IOrderStatus
{
    public string StatusName => "Shipped";

    public void ProcessOrder()
    {
        Console.WriteLine("Order is already shipped. Cannot process.");
    }

    public void ShipOrder()
    {
        Console.WriteLine("Order is already shipped. Cannot ship again.");
    }

    public void DeliverOrder()
    {
        Console.WriteLine("Delivering the order...");
        // Transition to Delivered status can be handled here
    }

    public void CancelOrder()
    {
        Console.WriteLine("Order is already shipped. Cannot cancel.");
    }
}