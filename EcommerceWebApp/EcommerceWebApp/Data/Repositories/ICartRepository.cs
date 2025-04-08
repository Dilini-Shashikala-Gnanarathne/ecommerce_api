using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;

public interface ICartRepository
{
    Task<Cart> AddToCartAsync(Cart item);

    Task<Cart> GetCartAsync(string username);

    Task<List<CartWithItemsDto>> GetCartWithItemsByUserNicAsync(string userNic);

    Task<int> GetAbandonedCartsAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetCartAbandonmentRateAsync(DateTime startDate, DateTime endDate);

}
