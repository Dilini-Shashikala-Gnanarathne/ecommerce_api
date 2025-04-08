using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDBContext _context;

    public ProductRepository(EcommerceDBContext context)
    {
        _context = context;
    }

    // Get all products with only required fields using projection and AsNoTracking for better read performance
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .AsNoTracking() // Avoid tracking for read-only query
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            })
            .ToListAsync();
    }

    // Get a single product by ID using FindAsync which is optimized for primary key lookup
    public async Task<Product> GetProductByIdAsync(long id)
    {
        return await _context.Products.FindAsync(id);
    }

    // Create a new product and save it to the database
    public async Task<Product> CreateProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    // Update an existing product
    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    // Delete a product by ID
    public async Task<bool> DeleteProductAsync(long id)
    {
        var product = await GetProductByIdAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    // Get products with pagination - useful for large datasets
    public async Task<List<Product>> GetPagedProductsAsync(int pageNumber, int pageSize)
    {
        return await _context.Products
            .AsNoTracking() // Again, skip change tracking for performance
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Get all discounted products using raw SQL (optional for complex logic)
    public async Task<List<Product>> GetDiscountedProductsAsync()
    {
        return await _context.Products
            .FromSqlRaw("SELECT * FROM Products WHERE Discount > 0")
            .AsNoTracking() // Even for raw SQL, we can avoid tracking
            .ToListAsync();
    }
}
