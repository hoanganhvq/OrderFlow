using BuildingBlocks.Common.Results;
using MyStoreProject.Services.Ordering.Domain.Errors;

namespace MyStoreProject.Services.Ordering.Domain.Entities;

public class OrderItem 
{
    public long Id { get; private set; }
    public Guid OrderId { get; private set; }
    public  string Sku { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    
    public OrderItem(){}
    
    public OrderItem(Guid orderId, string sku, decimal price, int quantity) 
    {
        OrderId = orderId;
        Sku = sku;
        UnitPrice = price;
        Quantity = quantity;
    }

    public Result UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            return OrderErrors.InvalidQuantity; 
        };
        Quantity = newQuantity;
        return Result.Success();
    }
    
    
}