
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class PermissionAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly HashSet<string> _requiredPermissions;

    public PermissionAuthorizationAttribute(params string[] requiredPermissions)
    {
        _requiredPermissions = new HashSet<string>(requiredPermissions ?? throw new ArgumentNullException(nameof(requiredPermissions)), StringComparer.OrdinalIgnoreCase);
        if (!_requiredPermissions.Any()) throw new ArgumentException("At least one permission must be provided", nameof(requiredPermissions));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;
        Console.WriteLine("User: " + user?.Identity);
        if (user?.Identity?.IsAuthenticated != true)
        {
            SetUnauthorizedResponse(context, "You are unauthorized.");
            return;
        }

        string permissionsClaim = user.Claims.FirstOrDefault(c => c.Type == "PermissionCodes")?.Value;
        if (string.IsNullOrWhiteSpace(permissionsClaim))
        {
            SetUnauthorizedResponse(context, "Invalid token: Missing permission claims.");
            return;
        }

        HashSet<string> userPermissions = new HashSet<string>(permissionsClaim.Split(',').Select(p => p.Trim()), StringComparer.OrdinalIgnoreCase);
        if (!_requiredPermissions.Overlaps(userPermissions))
        {
            SetForbiddenResponse(context, "Access denied. You do not have the required permissions.");
        }
    }

    private static void SetUnauthorizedResponse(AuthorizationFilterContext context, string message)
    {
        context.Result = new UnauthorizedObjectResult(ApiResponse<object>.ErrorResponse(message, Array.Empty<object>()));
    }

    private static void SetForbiddenResponse(AuthorizationFilterContext context, string message)
    {
        context.Result = new ObjectResult(ApiResponse<object>.ErrorResponse(message, Array.Empty<object>()))
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}
