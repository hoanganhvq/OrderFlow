namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderRequest (string CustomerId, List<OrderItemDTO> Items) { }