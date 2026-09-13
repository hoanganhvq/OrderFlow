namespace MyStoreProject.Services.Ordering.Domain.Entities;

public class OrderSagaState 
{
    private OrderSagaState(){}

    public OrderSagaState(Guid orderId)
    {
        OrderId = orderId;
    }
    
    public Guid OrderId { get; set; }
    public bool ReservationCompleted { get; set; } = false;
    public bool PaymentCompleted { get; set; } = false;
    
    public Guid? LastProcessedEventId { get; set; }
    
    public void MarkReservationCompleted(Guid eventId)
    {
        ReservationCompleted = true;
        LastProcessedEventId = eventId;
    }

    public void MarkPaymentCompleted(Guid eventId)
    {
        PaymentCompleted = true;
        LastProcessedEventId = eventId;
    }

    public void RecordEvent(Guid eventId)
    {
        LastProcessedEventId = eventId;
    }
    
}