using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/Product
    [HttpGet]
    [PermissionAuthorization(Permissions.ViewProductsCust, Permissions.ViewProductsAdmin)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(ApiResponse<List<ProductResponse>>.SuccessResponse("Products retrieved successfully"));
    }

    // GET: api/Product/{id}
    [HttpGet("{id:long}")]
    [PermissionAuthorization(Permissions.ViewProductsCust, Permissions.ViewProductsAdmin)]
    public async Task<IActionResult> GetProductById(long id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse($"Product with ID {id} not found"));
        }

        return Ok(ApiResponse<ProductResponse>.SuccessResponse("Product retrieved successfully", product));
    }

    // POST: api/Product
    [HttpPost]
    [PermissionAuthorization(Permissions.ViewProductsCust, Permissions.ViewProductsAdmin)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid product data"));
        }

        var product = await _productService.CreateProductAsync(request);

        return product == null
            ? BadRequest(ApiResponse<string>.ErrorResponse("Failed to create product"))
            : Ok(ApiResponse<ProductResponse>.SuccessResponse("Product created successfully", product));
    }
}
