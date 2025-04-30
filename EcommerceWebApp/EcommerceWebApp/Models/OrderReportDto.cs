namespace EcommerceWebApp.Models
{
    namespace EcommerceWebApp.Dtos
    {
        public class OrderReportDto
        {
            public string OrderId { get; set; }
            public string OrderName { get; set; }
            public decimal OrderTotalPrice { get; set; }
            public string OrderStatus { get; set; }
            public DateTime OrderCreatedAt { get; set; }
            public string OrderCreatedBy { get; set; }
            public string? OrderUserNic { get; set; }

            public long ProductId { get; set; }
            public string OrderItemProductName { get; set; }
            public int OrderItemQuantity { get; set; }
            public decimal OrderItemUnitPrice { get; set; }
            public decimal OrderItemTotalPrice { get; set; }

            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
          
        }
    }

}
