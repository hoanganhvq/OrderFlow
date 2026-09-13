using System.Net.Http.Json;
using MyStoreProject.BlazorApp.Models.Orders;
using MyStoreProject.BlazorApp.Models.Inventory;

namespace MyStoreProject.BlazorApp.Services;

public class OrderFlowClient(HttpClient http, IConfiguration configuration)
{
    private readonly string _ordersUrl = configuration["OrdersApi"];
    private readonly string _inventoryUrl = configuration["InventoryApi"];


    public async Task<OrderResponse> PlaceOrderAsync(OrderRequest request)
    {
        var res = await http.PostAsJsonAsync($"{_ordersUrl}/orders", request);
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }
        return await res.Content.ReadFromJsonAsync<OrderResponse>();
    }

    public async Task<OrderDetail?> GetOrderAsync(Guid id)
    {
        var result = await http.GetFromJsonAsync<OrderDetail>($"{_ordersUrl}/orders/{id}");
        return result;
    } 
    
    public async Task<List<OrderSummary>?> GetOrdersByCustomerAsync(string customerId)
    {
        var result = await http.GetFromJsonAsync<List<OrderSummary>>($"{_ordersUrl}/orders?customerId={customerId}");
        return result;

    }

    public async Task<List<InventoryDTO>?> GetStockAsync()
    {
        var result = await http.GetFromJsonAsync<List<InventoryDTO>>($"{_inventoryUrl}/stock");
        return result;
    }
}