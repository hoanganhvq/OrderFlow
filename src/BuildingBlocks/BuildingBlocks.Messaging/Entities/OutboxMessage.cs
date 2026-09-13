namespace BuildingBlocks.Messaging.Entities;

public class OutboxMessage 
{
    private OutboxMessage(){}

    public OutboxMessage(Guid id, Guid eventId, string topic, string payload)
    {
        Id = id;
        EventId = eventId;
        Topic = topic;
        Payload = payload;
    }
    public Guid Id { get;  private set; }
    public Guid EventId { get;  private set; }
    public string Topic { get;  private set;}  = string.Empty;
    public string Payload { get; private set; } =  string.Empty;
    public DateTime CreatedAt { get; private set; } =  DateTime.UtcNow;
    public DateTime? PublishedAt { get; private set; }
    
    public bool IsPublished => PublishedAt.HasValue;

    public void MarkAsPublished()
    {
        PublishedAt =  DateTime.UtcNow;
    }
    
}