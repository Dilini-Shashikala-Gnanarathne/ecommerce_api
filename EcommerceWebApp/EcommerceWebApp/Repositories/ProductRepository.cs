using EcommerceWebApp.EcommerceDBEntities;
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

        // Get all products asynchronously
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Get a single product by ID asynchronously
        public async Task<Product> GetProductByIdAsync(long id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
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

}
