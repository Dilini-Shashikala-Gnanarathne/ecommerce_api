using EcommerceWebApp.Models.EcommerceWebApp.Dtos;
using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly ISalesReportService _salesReportService;
    private readonly IOrderService _orderService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Constructor with correct dependency injection
    public ReportController(ISalesReportService salesReportService, IOrderService orderService, IHttpContextAccessor httpContextAccessor)
    {
        _salesReportService = salesReportService; // Fixed name
        _orderService = orderService;             // Inject IOrderService
        _httpContextAccessor = httpContextAccessor;
    }

    // Endpoint to get the sales performance report
    [HttpGet("sales-performance")]
    public async Task<IActionResult> GetSalesPerformanceReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var report = await _salesReportService.GetSalesPerformanceReport(startDate, endDate);
        return Ok(report);
    }

    // Endpoint to get the order report
    [HttpGet("report")]
    public async Task<ActionResult<List<OrderReportDto>>> GetOrderReport(
           [FromQuery] string status = "Pending",
           [FromQuery] DateTime startDate = default)
    {
        // If startDate is not provided, we can set a default value or handle it as needed
        if (startDate == default)
        {
            startDate = DateTime.Now.AddMonths(-1); // Example default: last month
        }

        Console.WriteLine($"Status: {status}");
        Console.WriteLine($"Start Date: {startDate}");

        var result = await _orderService.GetOrderReportAsync(status, startDate);

        // Handle the case where no results are found
        if (result == null || result.Count == 0)
        {
            return NotFound("No orders found matching the given criteria.");
        }

        return Ok(result);
    }
}
