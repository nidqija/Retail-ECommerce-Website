namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


//3. CONCRETE STATE CLASSES
// represents concrete states of an order, each implementing the IOrderStatus interface and 
// defining valid transitions to other states.
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