namespace MyStoreProject.Services.Ordering.API.DTO;

public record CreateOrderItemRequest
{
 
    public string Sku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}