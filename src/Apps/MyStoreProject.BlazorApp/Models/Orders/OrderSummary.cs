namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderSummary(Guid Id, string CustomerId, string Status, decimal TotalAmount, DateTime CreatedAt) { }