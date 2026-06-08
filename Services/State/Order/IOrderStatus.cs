namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;



// 1. Abstract State Interface
// this interface defines the contract for all order states, 
// ensuring that each state can determine if it can transition to another state.
public interface IOrderStatus
{
    string StatusName { get; }

    bool CanTransitionToNextStatus(string targetStatusName);
}