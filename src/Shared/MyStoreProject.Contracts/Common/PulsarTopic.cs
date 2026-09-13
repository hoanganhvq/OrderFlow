namespace MyStoreProject.Contracts.Common;

public static class PulsarTopics
{
    public const string OrderPlaced = "order-placed";
    public const string ReservationSucceeded = "reservation-succeeded";
    public const string ReservationFailed = "reservation-failed";
    public const string PaymentSucceeded = "payment-succeeded";
    public const string PaymentFailed = "payment-failed";
    public const string StockReleased = "stock-released";
}