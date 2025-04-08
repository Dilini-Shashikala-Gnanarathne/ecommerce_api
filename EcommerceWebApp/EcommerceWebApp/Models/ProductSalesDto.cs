namespace EcommerceWebApp.Models
{
    public class ProductSalesDto
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalSales { get; internal set; }
        public int QuantitySold { get; internal set; }
    }
}
