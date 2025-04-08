using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
public interface IOrderRepository
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetUserOrdersAsync(string username);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(string orderId);

    Task<Order> UpdateOrderAsync(Order order);

    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
    Task<List<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate);
    Task<List<CustomerSalesDto>> GetTopCustomersAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetAverageOrderValueAsync(DateTime startDate, DateTime endDate);

    //Task<IEnumerable<OrderItemData>> GetOrderItemDataAsync(DateTime startDate, DateTime endDate);
}


