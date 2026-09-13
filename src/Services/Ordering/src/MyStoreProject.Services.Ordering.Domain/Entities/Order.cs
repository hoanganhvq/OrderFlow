using BuildingBlocks.Common;
using BuildingBlocks.Common.Results;
using MyStoreProject.Services.Ordering.Domain.Enums;
using MyStoreProject.Services.Ordering.Domain.Errors;

namespace MyStoreProject.Services.Ordering.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; private set; } =  DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } =  DateTime.UtcNow;
    public byte[] RowVersion { get; private set; } = default!;
    private Order() { }
    
    private Order(string customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
    }
    
    public static Result<Order> Create(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return OrderErrors.InvalidField("CustomerId");
        }
        var order = new Order(customerId);
        return Result<Order>.Success(order);       
    }

    public Result AddOrderItem(string sku, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
        {
            return OrderErrors.CannotAddItems;
        }

        var existingLine = _orderItems.FirstOrDefault(i => i.Sku == sku);
        if (existingLine != null)
        {
            var updateResult = existingLine.UpdateQuantity(existingLine.Quantity + quantity);
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }
        }
        else
        {
            _orderItems.Add(new OrderItem(Id, sku, unitPrice, quantity));
        }

        CalculateTotalPrice();
        return Result.Success();
    }

    public Result RemoveOrderItem(string sku)
    {
        if (Status != OrderStatus.Pending)
        {
            return OrderErrors.CannotRemoveItems;
        }   
        var existingItem = _orderItems.FirstOrDefault(i => i.Sku == sku);
        if (existingItem is null)
        {
            return OrderErrors.ItemNotFound;
        }
        _orderItems.Remove(existingItem);
        CalculateTotalPrice();
        return Result.Success();
    }

    
    public Result SubmitOrder()
    {
        if (!_orderItems.Any())
        {
            return OrderErrors.EmptyItems;
        }
        return Result.Success();
    }
    
    public void CalculateTotalPrice()
    {
        TotalPrice = _orderItems.Sum(item => item.UnitPrice * item.Quantity);
    }
    
    
    public Result MarkAsReserving()
    {
        if (Status != OrderStatus.Pending)
        {
            return OrderErrors.CannotChangStatus;
        }
        Status = OrderStatus.Reserving;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
        
    }

    public Result MarkAsCharging()
    {
        if (Status != OrderStatus.Reserving && Status != OrderStatus.Pending)
        {
            return OrderErrors.CannotChangStatus;
        }
        Status = OrderStatus.Charging;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    
    public Result MarkAsConfirmed()
    {
        if (Status != OrderStatus.Charging)
        {
            return OrderErrors.CannotChangStatus;
        }
        Status = OrderStatus.Confirmed;  
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == OrderStatus.Confirmed || Status == OrderStatus.Cancelled)
        {
            return OrderErrors.CannotChangStatus;
        }
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();    
    }
    
}
