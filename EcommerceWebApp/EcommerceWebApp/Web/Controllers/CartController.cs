
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [PermissionAuthorization(Permissions.ViewCart)]
    public async Task<IActionResult> GetCart()
    {
        CartResponse cart = await _cartService.GetCart();
        return Ok(ApiResponse<CartResponse>.SuccessResponse("Cart retrieved successfully", cart));
    }

    [HttpPost]
    [PermissionAuthorization(Permissions.AddToCart)]
    public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
    {
        CartResponse cart = await _cartService.AddToCart( request);
        Console.WriteLine(cart.ToString());

        if (cart == null)
        {
            return BadRequest(ApiResponse<CartResponse>.ErrorResponse("Failed to add item to cart"));
        }

        return Ok(ApiResponse<CartResponse>.SuccessResponse("Item added to cart successfully", cart));
    }

    [HttpGet("cart")]
    [PermissionAuthorization(Permissions.AddToCart)]
    public async Task<IActionResult> GetCartDetails()
    {
        try
        {
            var cartResponse = await _cartService.GetCartWithItems(); // Uses stored procedure
            return Ok(new
            {
                success = true,
                message = "Cart retrieved successfully",
                data = cartResponse
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Internal server error"
            });
        }
    }


}
