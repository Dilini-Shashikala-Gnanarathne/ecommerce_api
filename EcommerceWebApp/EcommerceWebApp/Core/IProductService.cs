using EcommerceWebApp.Models;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductResponse> GetProductByIdAsync(long id);
    Task<ProductResponse> CreateProductAsync(ProductRequest request);

    Task<ProductResponse> UpdateProductAsync(long id, ProductRequest request);
}
