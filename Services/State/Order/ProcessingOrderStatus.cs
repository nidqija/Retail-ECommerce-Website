namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class ProcessingOrderStatus : IOrderStatus
{
    public string StatusName => "Processing";

    public void ProcessOrder()
    {
        Console.WriteLine("Order is already being processed.");
    }

    public void ShipOrder()
    {
        Console.WriteLine("Shipping order...");
        // Transition to Shipped status
    }

    public void DeliverOrder()
    {
        Console.WriteLine("Cannot deliver. Order is still processing.");
    }

    public void CancelOrder()
    {
        Console.WriteLine("Cancelling order...");
        // Transition to Cancelled status
    }
}