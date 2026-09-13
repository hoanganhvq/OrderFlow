namespace BuildingBlocks.Messaging.Entities;

public class InboxMessage
{
    private InboxMessage(){}
    
    public InboxMessage(Guid eventId)
    {
        EventId = eventId;
    }
    public Guid EventId { get; }
    public DateTime ProcessedAt { get; } =  DateTime.UtcNow;
}