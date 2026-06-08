namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;




// 2. State Manager Class
// this class manages the current state of an order and handles state transitions,
// ensuring that transitions are valid according to the rules defined in each state class.
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
        // declare concrete states based on the order's current status, 
        // defaulting to Pending if the status is unrecognized
        {
            "Processing" => new ProcessingOrderStatus(),
            "Shipped" => new ShippedOrderStatus(),
            "Completed" => new CompletedOrderStatus(),
            "Cancelled" => new CancelledOrderStatus(),
            _ => new PendingOrderStatus()

        };
    }

    // method to transition to a new state, 
    // checking if the transition is valid before updating the order's status
    public void TransitionTo(string targetStatusName)
    {
        if(!CurrentState.CanTransitionToNextStatus(targetStatusName))
        {
            throw new InvalidOperationException($"Cannot transition from {CurrentState.StatusName} to {targetStatusName}.");
        };


        _order.OrderStatus = targetStatusName;
    }
    
    
    
   

}