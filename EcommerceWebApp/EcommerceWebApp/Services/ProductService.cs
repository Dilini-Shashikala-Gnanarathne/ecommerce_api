using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

public class ProductService : IProductService
{
    private readonly EcommerceDBContext _context;
    private readonly CustomContext _customContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger; // Inject ILogger

    public ProductService(EcommerceDBContext context, CustomContext customContext, IMapper mapper, ILogger<ProductService> logger)
    {
        _context = context;
        _customContext = customContext;
        _mapper = mapper;
        _logger = logger; // Initialize logger
    }

    public async Task<List<ProductResponse>> GetAllProductsAsync()
    {
        _logger.LogInformation("Getting all products from the database");
        var products = await _context.Products.ToListAsync();
        _logger.LogInformation("Successfully retrieved {ProductCount} products", products.Count);
        return _mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<ProductResponse> GetProductByIdAsync(long id)
    {
        _logger.LogInformation("Fetching product with ID {ProductId}", id);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return null;
        }
        _logger.LogInformation("Successfully fetched product with ID {ProductId}", id);
        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
    {
        _logger.LogInformation("Creating a new product with name {ProductName}", request.Name);
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Category = request.Category,
            Images = request.Images.Count > 0 ? JsonConvert.SerializeObject(request.Images) : "[]",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _customContext.Username,
            UserNic = _customContext.NicNumber
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully created product with ID {ProductId}", product.Id);
        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> UpdateProductAsync(long id, ProductRequest request)
    {
        _logger.LogInformation("Updating product with ID {ProductId}", id);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return null;
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Category = request.Category;
        product.Images = request.Images.Count > 0 ? JsonConvert.SerializeObject(request.Images) : product.Images;
        product.UpdatedAt = DateTime.UtcNow;
        product.UserNic = _customContext.NicNumber;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully updated product with ID {ProductId}", id);
        return _mapper.Map<ProductResponse>(product);
    }
}
