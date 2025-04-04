using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class ProductService : IProductService
{
    private readonly EcommerceDBContext _context;
    private readonly CustomContext _customContext;
    private readonly IMapper _mapper;

    public ProductService(EcommerceDBContext context, CustomContext customContext, IMapper mapper)
    {
        _context = context;
        _customContext = customContext;
        _mapper = mapper;
    }

    public async Task<List<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        return _mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<ProductResponse> GetProductByIdAsync(long id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product != null ? _mapper.Map<ProductResponse>(product) : null;
    }

    public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
    {
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

        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> UpdateProductAsync(long id, ProductRequest request)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return null; 
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Category = request.Category;
        product.Images = request.Images.Count > 0 ? JsonConvert.SerializeObject(request.Images) : product.Images; // Serialize images if provided
        product.UpdatedAt = DateTime.UtcNow; 
        product.UserNic = _customContext.NicNumber;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductResponse>(product);
    }

}
