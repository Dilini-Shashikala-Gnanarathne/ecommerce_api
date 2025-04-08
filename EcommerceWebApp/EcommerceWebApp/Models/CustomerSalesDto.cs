namespace EcommerceWebApp.Models
{
    public class CustomerSalesDto
    {
        public string CustomerName { get; set; }
        public int NumberOfOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int OrdersCount { get; internal set; }
    }
}
