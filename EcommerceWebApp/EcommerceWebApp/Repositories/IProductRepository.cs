
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;

public interface IProductRepository
{

    Task<List<Product>> GetAllProductsAsync();
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(long id);
    Task<Product> GetProductByIdAsync(long productId);
}