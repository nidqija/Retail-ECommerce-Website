namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



public class OrderStateManager
{
    
    // make a reference to the order model to update the status in the database when transitioning states
    private readonly Order _order;

    // expose the current state to allow the UI or other services to check the current status of the order
    public IOrderStatus CurrentState { get; private set; }

    // initialize the state manager with the current order and set the initial state based on the order's status
    public OrderStateManager(Order order)
    {
        _order = order;

        CurrentState = _order.OrderStatus switch
        {
            "Processing" => new ProcessingOrderStatus(),
            "Shipped" => new ShippedOrderStatus(),
            "Completed" => new CompletedOrderStatus(),
            "Cancelled" => new CancelledOrderStatus(),
            _ => new PendingOrderStatus()

        };
    }

    public void TransitionTo(string targetStatusName)
    {
        if(!CurrentState.CanTransitionToNextStatus(targetStatusName))
        {
            throw new InvalidOperationException($"Cannot transition from {CurrentState.StatusName} to {targetStatusName}.");
        };


        _order.OrderStatus = targetStatusName;
    }
    
    
    
   

}