using Microsoft.AspNetCore.Mvc;
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
            return Ok(ApiResponse<List<ProductResponse>>.SuccessResponse("Products retrieved successfully", products));
        }

        // POST: api/Product
        [HttpPost]
        [PermissionAuthorization(Permissions.ViewProductsCust)]
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
