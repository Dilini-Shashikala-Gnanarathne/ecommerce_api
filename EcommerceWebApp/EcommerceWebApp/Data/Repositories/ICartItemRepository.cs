using EcommerceWebApp.EcommerceDBEntities;

public interface ICartItemRepository
{
    //Task<CartItem> GetCartItemAsync(string username);
    Task<CartItem> AddToCartItemAsync(CartItem item);
}
