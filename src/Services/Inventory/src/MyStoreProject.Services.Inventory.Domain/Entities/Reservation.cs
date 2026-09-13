using BuildingBlocks.Common;
using BuildingBlocks.Common.Results;
using MyStoreProject.Services.Inventory.Domain.Enum;
using MyStoreProject.Services.Inventory.Domain.Errors;

namespace MyStoreProject.Services.Inventory.Domain.Entities;

public class Reservation 
{
    public long Id { get; private set; }
    
    public Guid OrderId { get; private set; }
    
    public string Sku { get; private  set; }
    
    public int Quantity { get; private set; }
    
    public ReservationStatus Status { get; private set; } = ReservationStatus.Active;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Reservation() { }
    
    public static Result<Reservation> Create(Guid orderId, string sku, int quantity)
    {
        if (orderId == Guid.Empty)
        {
            return ReservationErrors.InvalidField("OrderId");
        }
            
        if (string.IsNullOrWhiteSpace(sku))
            return ReservationErrors.InvalidField("Sku");

        if (quantity <= 0)
            return ReservationErrors.InvalidField("Quantity");

        var reservation = new Reservation
        {
            OrderId = orderId,
            Sku = sku,
            Quantity = quantity,
            Status = ReservationStatus.Active
        };

        return Result<Reservation>.Success(reservation);
    }
    
    public Result Consume()
    {
        if (Status != ReservationStatus.Active)
        {
            return ReservationErrors.CannotChangeStatus;
        }

        Status = ReservationStatus.Consumed;
        return Result.Success();
    }

    public Result Release()
    {
        if (Status != ReservationStatus.Active)
        {
            return ReservationErrors.CannotChangeStatus;
        }
        Status = ReservationStatus.Released;
        return Result.Success();
    }
    
    public Result IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            return ReservationErrors.InvalidField("Quantity");
        }
        Quantity += quantity;
        return Result.Success();
    }
}