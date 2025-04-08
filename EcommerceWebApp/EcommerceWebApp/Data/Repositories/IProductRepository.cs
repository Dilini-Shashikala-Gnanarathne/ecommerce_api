
using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;

public interface IProductRepository
{

    Task<List<ProductDto>> GetAllProductsAsync();
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(long id);
    Task<Product> GetProductByIdAsync(long productId);
}