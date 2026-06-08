namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class CompletedOrderStatus : IOrderStatus
{
    public string StatusName => "Completed";

    public bool CanTransitionToNextStatus(string targetStatusName)
    {
        Console.WriteLine($"Attempting to transition from {StatusName} to {targetStatusName}.");
        Console.WriteLine("Transition not allowed. Order is already completed.");
        return false;
    }
}