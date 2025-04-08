public interface ICartService
{
    Task<CartResponse> GetCart();
    Task<CartResponse> AddToCart(CartRequest request);

    Task<CartResponse> GetCartWithItems();
}
