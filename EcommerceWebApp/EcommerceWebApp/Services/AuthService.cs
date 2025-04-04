using EcommerceWebApp.BaseDBEntities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;

    }

    public LoginResponse Login(LoginRequest request)
    {
        // Find user by username
        User user = _userRepository.GetUserByUsername(request.Username);
        if (user == null || user.Password != request.Password) // In production, use password hashing
        {
            return null; // Authentication failed
        }

        // Generate token
        string token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        // Check if username already exists
        if (_userRepository.GetUserByUsername(request.Username) != null)
        {
            return null; // Username already taken
        }

        // Check if email already exists
        if (_userRepository.GetUserByEmail(request.Email) != null)
        {
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

        return new RegisterResponse
        {
            Username = createdUser.Username,
            Email = createdUser.Email,
            Role = createdUser.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
