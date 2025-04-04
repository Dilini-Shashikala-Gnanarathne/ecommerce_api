public interface IProductService
{
    Task<List<ProductResponse>> GetAllProductsAsync();
    Task<ProductResponse> GetProductByIdAsync(long id);
    Task<ProductResponse> CreateProductAsync(ProductRequest request);
}
