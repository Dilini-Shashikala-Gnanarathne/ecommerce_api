public class CartResponse
{
    public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
    public decimal TotalPrice { get; set; }
}