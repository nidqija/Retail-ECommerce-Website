namespace RetailECommerce.Services.State.Order;
using RetailECommerce.Models;


public interface IOrderStatus
{
    string StatusName { get; }

    void ProcessOrder();

    void ShipOrder();

    void DeliverOrder();

    void CancelOrder();
}