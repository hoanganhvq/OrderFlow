namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderDetail
{
    
    public Guid OrderId { get; }
    public string CustomerId { get; }
    public string Status { get;  }
    public decimal TotalAmount { get;  }
    public bool ReservationCompleted { get;  }
    public bool PaymentCompleted { get;  }
    public List<OrderItemDTO> Items { get;  }
    public DateTime CreatedAt { get;  }
    public DateTime UpdatedAt { get;  }
}