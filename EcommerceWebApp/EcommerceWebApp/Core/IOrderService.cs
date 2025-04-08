
public interface IOrderService
{
    Task<List<OrderResponse>> GetAllOrdersAsync();
    Task<List<OrderResponse>> GetUserOrdersAsync();
    Task<OrderResponse> CreateOrderAsync(OrderRequest request);
    Task<OrderResponse> UpdateOrderAsync(string orderId, OrderRequest request); // Fix: Accept OrderRequest
}

