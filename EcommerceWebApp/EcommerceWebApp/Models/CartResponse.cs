using EcommerceWebApp.EcommerceDBEntities;

public class CartResponse
{
    public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
    
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UserNic { get; set; }
}