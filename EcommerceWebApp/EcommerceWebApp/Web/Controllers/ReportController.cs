

using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly ISalesReportService _salesReportService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportController(ISalesReportService cartService, IHttpContextAccessor httpContextAccessor)
    {
        _salesReportService = cartService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("sales-performance")]
    public async Task<IActionResult> GetSalesPerformanceReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var report = await _salesReportService.GetSalesPerformanceReport(startDate, endDate);
        return Ok(report);
    }
}
