
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var response = _authService.Login(request);
        if (response == null)
        {
            return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid username or password"));
        }
        return Ok(ApiResponse<LoginResponse>.SuccessResponse("Login successful", response));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var response = _authService.Register(request);
        if (response == null)
        {
            return BadRequest(ApiResponse<RegisterResponse>.ErrorResponse("Username or email already exists"));
        }
        return Ok(ApiResponse<RegisterResponse>.SuccessResponse("Registration successful", response));
    }
}
