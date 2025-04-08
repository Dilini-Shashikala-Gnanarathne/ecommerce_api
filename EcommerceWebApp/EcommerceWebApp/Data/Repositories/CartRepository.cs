
using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CartRepository : ICartRepository
{
    private readonly EcommerceDBContext _context;

    public CartRepository(EcommerceDBContext context)
    {
        _context = context;
    }

    // Retrieve the user's cart asynchronously
    public async Task<Cart> GetCartAsync(string username)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .Where(c => c.UserNic == username)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }

    // Add an item to the user's cart asynchronously
    public async Task<Cart> AddToCartAsync(Cart item)
    {
        try
        {
            await _context.Carts.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., log and rethrow
            throw new InvalidOperationException("Failed to add item to cart", ex);
        }
    }

    public async Task<List<CartWithItemsDto>> GetCartWithItemsByUserNicAsync(string userNic)
    {
        return await _context.Set<CartWithItemsDto>()
            .FromSqlRaw("EXEC GetUserCartWithItems @UserNic = {0}", userNic)
            .ToListAsync();
    }

    public async Task<int> GetAbandonedCartsAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Carts
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate && c.Id == null)
            .CountAsync();
    }

    public async Task<decimal> GetCartAbandonmentRateAsync(DateTime startDate, DateTime endDate)
    {
        var totalCarts = await _context.Carts
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
            .CountAsync();

        var abandonedCarts = await GetAbandonedCartsAsync(startDate, endDate);

        return totalCarts == 0 ? 0 : (decimal)abandonedCarts / totalCarts * 100;
    }

}
