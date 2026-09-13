namespace MyStoreProject.Services.Ordering.API.DTO;

public record CreateOrderRequest
{
    public string CustomerId { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; }
}