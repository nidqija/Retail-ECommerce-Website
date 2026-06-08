namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public class CancelledOrderStatus : IOrderStatus
{
    public string StatusName => "Cancelled";

    public bool CanTransitionToNextStatus(string targetStatusName)
    {
        Console.WriteLine($"Attempting to transition from {StatusName} to {targetStatusName}.");
        Console.WriteLine("Transition not allowed. Order is already cancelled.");
        return false;
    }
}