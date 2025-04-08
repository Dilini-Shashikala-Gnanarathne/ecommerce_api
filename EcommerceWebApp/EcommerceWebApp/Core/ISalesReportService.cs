using EcommerceWebApp.Models;

public interface ISalesReportService
{
    Task<SalesReportDto> GetSalesPerformanceReport(DateTime startDate, DateTime endDate);
}
