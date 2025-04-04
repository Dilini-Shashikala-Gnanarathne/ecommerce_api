public class OrderRequest
{
    
        public string OrderName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();

    // RowVersion will be handled as a base64 string to work with concurrency
    public string RowVersion { get; set; } = string.Empty;

    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

}
