
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [PermissionAuthorization(Permissions.ViewUsers)]
    public IActionResult GetAllUsers()
    {
        List<UserResponse> users = _userService.GetAllUsers();
        return Ok(ApiResponse<List<UserResponse>>.SuccessResponse("Users retrieved successfully", users));
    }
}
