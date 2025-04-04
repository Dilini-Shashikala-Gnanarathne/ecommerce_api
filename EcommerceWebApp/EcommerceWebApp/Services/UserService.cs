using EcommerceWebApp.BaseDBEntities;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly CustomContext _customContext;

    public UserService(IUserRepository userRepository, CustomContext customContext)
    {
        _userRepository = userRepository;
        _customContext = customContext;
    }

    public List<UserResponse> GetAllUsers()
    {
        List<User> users = _userRepository.GetAllUsers();
        return users.Select(MapToUserResponse).ToList();
    }

    public UserResponse GetUserById(long id)
    {
        User user = _userRepository.GetUserById(id);
        return user != null ? MapToUserResponse(user) : null;
    }

    private UserResponse MapToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            NicNumber = user.NicNumber,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}
