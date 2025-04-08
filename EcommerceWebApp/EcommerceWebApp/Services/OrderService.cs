using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly CustomContext _customContext;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        CustomContext customContext,
        IMapper mapper,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customContext = customContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        _logger.LogInformation("Fetching all orders");
        List<Order> orders = await _orderRepository.GetAllOrdersAsync();
        return _mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<List<OrderResponse>> GetUserOrdersAsync()
    {
        string username = _customContext.Username;
        _logger.LogInformation("Fetching orders for user: {Username}", username);
        List<Order> orders = await _orderRepository.GetUserOrdersAsync(username);
        return _mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderRequest request)
    {
        string username = _customContext.Username;
        _logger.LogInformation("Creating order for user: {Username}", username);

        string orderId = await GenerateOrderIdAsync();

        List<OrderItem> orderItems = new List<OrderItem>();
        decimal totalPrice = 0;

        foreach (var item in request.Items)
        {
            Product product = await _productRepository.GetProductByIdAsync(item.ProductId);
            if (product == null || product.Stock < item.Quantity)
            {
                _logger.LogWarning("Product not found or insufficient stock for product ID {ProductId}.", item.ProductId);
                throw new InvalidOperationException("Product not found or insufficient stock.");
            }

            OrderItem orderItem = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };

            orderItems.Add(orderItem);
            totalPrice += product.Price * item.Quantity;

            // Update product stock asynchronously
            product.Stock -= item.Quantity;
        }

        // Create new order with unique ID
        Order order = new Order
        {
            OrderId = orderId,  // Use generated OrderId
            OrderName = request.OrderName,
            Price = totalPrice,
            Status = OrderStatus.Pending,
            CreatedBy = username,
            CreatedAt = DateTime.Now,
            OrderItems = orderItems
        };

        // Save to database
        await _orderRepository.CreateOrderAsync(order);

        _logger.LogInformation("Order created successfully for user: {Username}", username);
        return _mapper.Map<OrderResponse>(order);
    }

    private async Task<string> GenerateOrderIdAsync()
    {
        List<Order> orders = await _orderRepository.GetAllOrdersAsync();

        Order latestOrder = orders.OrderByDescending(o => o.OrderId).FirstOrDefault();

        if (latestOrder == null)
        {
            return "ORD-0001";
        }

        var lastOrderId = latestOrder.OrderId;
        int orderNumber;
        if (!int.TryParse(lastOrderId.Substring(4), out orderNumber))
        {
            throw new InvalidOperationException("Invalid OrderId format.");
        }

        orderNumber++;

        // Generate the new OrderId
        return $"ORD-{orderNumber:D4}";  // Format to 4 digits (e.g., 'ORD-0001')
    }

    // Update order asynchronously with optimistic concurrency
    public async Task<OrderResponse> UpdateOrderAsync(string orderId, OrderRequest request)
    {
        // Fetch the order by ID
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
            return null; // Order not found
        }

        // Convert the RowVersion (Base64 string) from the request to a byte array
        byte[] requestRowVersion = Convert.FromBase64String(request.RowVersion);

        // Check if the RowVersion matches
        if (!order.RowVersion.SequenceEqual(requestRowVersion))
        {
            _logger.LogWarning("The order was modified by another user.");
            throw new DbUpdateConcurrencyException("The order was modified by another user.");
        }

        // Update the fields (do not touch RowVersion)
        order.OrderName = request.OrderName;
        order.Price = request.Price;
        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        try
        {
            // Call the repository to save changes. EF will handle the RowVersion update automatically.
            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);

            _logger.LogInformation("Order updated successfully for ID {OrderId}.", orderId);
            // Return the response
            return _mapper.Map<OrderResponse>(updatedOrder);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception if it occurs during the save
            throw new Exception("The order was modified by another user. Please refresh and try again.");
        }
    }
}
