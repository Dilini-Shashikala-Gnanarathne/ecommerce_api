namespace EcommerceWebApp.Models
{
    public class CartWithItemsDto
    {
        public long CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UserNic { get; set; }
        public long? CartItemId { get; set; }
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }

}
