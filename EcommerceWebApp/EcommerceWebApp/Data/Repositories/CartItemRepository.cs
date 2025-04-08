
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CartItemRepository : ICartItemRepository
{
    private readonly EcommerceDBContext _context;

    public CartItemRepository(EcommerceDBContext context)
    {
        _context = context;
    }

    public async Task<CartItem> AddToCartItemAsync(CartItem item)
    {
        Console.WriteLine("Inside Repo");
        try
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., log and rethrow
            throw new InvalidOperationException("Failed to add item to cart", ex);
        }
    }

}
