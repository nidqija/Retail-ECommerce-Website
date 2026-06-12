namespace RetailECommerce.Models;

public enum NotificationType
{
    OrderUpdate,
    PaymentUpdate,
    SystemAlert,

    NewOrderReceived,
    NewCustomerEnquiry,
    NewCustomerReview,
    ProductOutOfStock
}