using System.Security.Claims;

// Custom context for accessing user claims
public class CustomContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Username => GetClaimValue(ClaimTypes.Name);
    public string NicNumber => GetClaimValue("NicNumber");
    public string Role => GetClaimValue(ClaimTypes.Role);
    public string UserId => GetClaimValue(ClaimTypes.NameIdentifier);

    public DateTime CurrentDateTime => DateTime.UtcNow;

    private string GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType) ?? string.Empty;
    }
}

// Error handling middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "An internal server error occurred.",
            DetailedMessage = exception.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}

// JWT authentication middleware extension
public static class JwtMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // JWT validation is handled by the authentication middleware
        // This middleware could be used for additional JWT processing if needed
        await _next(context);
    }
}
