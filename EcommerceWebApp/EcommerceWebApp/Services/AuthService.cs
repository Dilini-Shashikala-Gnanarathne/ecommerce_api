using EcommerceWebApp.BaseDBEntities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public LoginResponse Login(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        // Find user by username
        User user = _userRepository.GetUserByUsername(request.Username);
        if (user == null || user.Password != request.Password) // In production, use password hashing
        {
            _logger.LogWarning("Login failed for user: {Username}. User not found or invalid password.", request.Username);
            return null; // Authentication failed
        }

        // Generate token
        string token = GenerateJwtToken(user);

        _logger.LogInformation("Login successful for user: {Username}", request.Username);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for username: {Username}", request.Username);

        // Check if username already exists
        if (_userRepository.GetUserByUsername(request.Username) != null)
        {
            _logger.LogWarning("Registration failed for username: {Username}. Username already taken.", request.Username);
            return null; // Username already taken
        }

        // Check if email already exists
        if (_userRepository.GetUserByEmail(request.Email) != null)
        {
            _logger.LogWarning("Registration failed for email: {Email}. Email already registered.", request.Email);
            return null; // Email already registered
        }

        // Create new user
        User user = new User
        {
            Username = request.Username,
            Password = request.Password, // In production, hash this
            Email = request.Email,
            NicNumber = request.NicNumber,
            Role = Roles.Customer, // Default role
            CreatedAt = DateTime.UtcNow, // Set creation date/time
            CreatedBy = "system",
            UserNic = request.NicNumber // Handle optional UserNic property
        };

        // Save user
        User createdUser = _userRepository.CreateUser(user);

        _logger.LogInformation("Registration successful for user: {Username}", createdUser.Username);

        return new RegisterResponse
        {
            Username = createdUser.Username,
            Email = createdUser.Email,
            Role = createdUser.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        _logger.LogInformation("Generating JWT token for user: {Username}", user.Username);

        string jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        string[] permissionCodes = user.Role switch
        {
            "Admin" => new[] {
                Permissions.ViewAllOrders,
                Permissions.ViewUserOrders,
                Permissions.ViewProductsAdmin,
                Permissions.CreateProduct,
                Permissions.ViewUsers
            },
            "Customer" => new[] {
                Permissions.ViewCart,
                Permissions.AddToCart,
                Permissions.ViewUserOrders,
                Permissions.CreateOrder,
                Permissions.ViewProductsCust
            },
            _ => Array.Empty<string>()
        };

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("NicNumber", user.NicNumber),
            new Claim("PermissionCodes", string.Join(",", permissionCodes))
        };

        // Add optional UserNic claim if it exists
        if (!string.IsNullOrEmpty(user.UserNic))
        {
            claims.Add(new Claim("UserNic", user.UserNic));
        }

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("JWT token generated for user: {Username}", user.Username);

        return jwtToken;
    }
}
