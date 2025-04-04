public class OrderResponse
{
    public string OrderId { get; set; } = string.Empty;
    public string OrderName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public string UserNic { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    public byte[] RowVersion { get;  set; }
}