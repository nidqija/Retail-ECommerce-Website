namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public interface IOrderStatus
{
    string StatusName { get; }

    bool CanTransitionToNextStatus(string targetStatusName);
}