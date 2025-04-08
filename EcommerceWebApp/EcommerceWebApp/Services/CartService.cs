using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartService> _logger;
    private readonly IMapper _mapper;
    private readonly CustomContext _customContext;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ILogger<CartService> logger,
        CustomContext customContext,
        ICartItemRepository cartItemRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
        _customContext = customContext;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartResponse> GetCart()
    {
        string username = _customContext.NicNumber;
        _logger.LogInformation("Fetching cart for user: {Username}", username);

        Cart cart = await _cartRepository.GetCartAsync(username);

        if (cart == null)
        {
            throw new InvalidOperationException($"No cart found for user: {username}");
        }

        return _mapper.Map<CartResponse>(cart);
    }


    public async Task<CartResponse> AddToCart(CartRequest request)
    {
        _logger.LogInformation("Adding item to cart for user: {Username}", _customContext.Username);

        Product product = await _productRepository.GetProductByIdAsync(request.ProductId);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} does not exist.", request.ProductId);
            throw new InvalidOperationException($"Product with ID {request.ProductId} does not exist.");
        }

        // Create cart first
        Cart cart = new Cart
        {
            CreatedAt = DateTime.Now,
            UserNic = _customContext.NicNumber,
            CreatedBy = _customContext.UserId,
        };
        Cart savedCart = await _cartRepository.AddToCartAsync(cart);
        if (savedCart == null)
        {
            _logger.LogError("Failed to create cart for user: {UserNic}", _customContext.NicNumber);
            throw new InvalidOperationException("Failed to create cart");
        }
        _logger.LogError("Successed to create cart for user: {UserNic}", _customContext.NicNumber);

        // Create cart item now that cart exists
        CartItem cartItem = new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = request.Quantity,
            UnitPrice = product.Price,
            CartId = savedCart.Id // use actual Cart ID
        };

        CartItem savedCartItem = await _cartItemRepository.AddToCartItemAsync(cartItem);
        savedCart.CartItems = new List<CartItem> { savedCartItem };
        return _mapper.Map<CartResponse>(savedCart);
    }

    public async Task<CartResponse> GetCartWithItems()
    {
        string userNic = _customContext.NicNumber;
        var result = await _cartRepository.GetCartWithItemsByUserNicAsync(userNic);

        if (result == null || !result.Any())
            throw new InvalidOperationException("No cart found for user.");

        var cart = result.First();

        return new CartResponse
        {
            Id = cart.CartId,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            CreatedBy = cart.CreatedBy,
            UserNic = cart.UserNic,
            Items = result
                .Where(r => r.CartItemId.HasValue)
                .Select(r => new CartItemResponse
                {
                    ProductId = r.ProductId.Value,
                    ProductName = r.ProductName,
                    Quantity = r.Quantity.Value,
                    UnitPrice = r.UnitPrice.Value
                }).ToList()
        };
    }


}
