using System.Text.Json.Serialization;

namespace MyStoreProject.BlazorApp.Models.Orders;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending = 0,
    Reserving = 1,
    Charging = 2,
    Confirmed = 3,
    Cancelled = 4
}