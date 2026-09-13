namespace MyStoreProject.Contracts.Common;

public class PulsarSubscriptions
{
    public const string InventoryOrderPlaced = "inventory-order-placed-sub";
    public const string InventoryPaymentSucceeded = "inventory-payment-succeeded-sub";
    public const string InventoryPaymentFailed = "inventory-payment-failed-sub";
    
    public const string OrdersReservationSucceeded = "orders-reservation-succeeded-sub";
    public const string OrdersReservationFailed = "orders-reservation-failed-sub";
    public const string OrdersPaymentSucceeded = "orders-payment-succeeded-sub";
    public const string OrdersPaymentFailed = "orders-payment-failed-sub";

    public const string PaymentsReservationSucceeded = "payments-reservation-succeeded-sub";
}