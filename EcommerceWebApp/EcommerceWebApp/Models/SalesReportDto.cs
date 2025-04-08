namespace EcommerceWebApp.Models
{
    public class SalesReportDto
    {
        public decimal TotalRevenue { get; set; }
        public List<ProductSalesDto> TopSellingProducts { get; set; }
        public List<CustomerSalesDto> TopCustomers { get; set; }
        public int AbandonedCarts { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal CartAbandonmentRate { get; set; }
    }

}
