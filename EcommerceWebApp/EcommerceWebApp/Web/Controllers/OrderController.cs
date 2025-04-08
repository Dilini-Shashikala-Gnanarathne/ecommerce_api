
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Async method to get all orders
    [HttpGet]
    [PermissionAuthorization(Permissions.ViewUserOrders)]
    public async Task<IActionResult> GetAllOrders()
    {
        List<OrderResponse> orders = await _orderService.GetAllOrdersAsync();  // Async service call
        return Ok(ApiResponse<List<OrderResponse>>.SuccessResponse("Orders retrieved successfully", orders));
    }

    // Async method to create an order
    [HttpPost]
    [PermissionAuthorization(Permissions.CreateOrder)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
    {
        OrderResponse order = await _orderService.CreateOrderAsync(request);  // Async service call

        if (order == null)
        {
            return BadRequest(ApiResponse<OrderResponse>.ErrorResponse("Failed to create order"));
        }

        return Ok(ApiResponse<OrderResponse>.SuccessResponse("Order created successfully", order));
    }

    // Async method to update an order
    [HttpPut("{orderId}")]
    [PermissionAuthorization(Permissions.CreateOrder)]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderRequest request)
    {
        try
        {
            // Update order using the service method
            OrderResponse updatedOrder = await _orderService.UpdateOrderAsync(orderId, request);

            if (updatedOrder == null)
            {
                return NotFound(ApiResponse<OrderResponse>.ErrorResponse("Order not found"));
            }

            return Ok(ApiResponse<OrderResponse>.SuccessResponse("Order updated successfully", updatedOrder));
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(ApiResponse<string>.ErrorResponse("The order was modified by another user. Please refresh and try again."));
        }
    }

}
