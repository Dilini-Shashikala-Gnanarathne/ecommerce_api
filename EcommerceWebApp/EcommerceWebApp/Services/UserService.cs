using AutoMapper;
using EcommerceWebApp.BaseDBEntities;
using Microsoft.Extensions.Logging;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly CustomContext _customContext;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, CustomContext customContext, ILogger<UserService> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _customContext = customContext;
        _logger = logger;
        _mapper = mapper;
    }

    public List<UserResponse> GetAllUsers()
    {
        _logger.LogInformation("Fetching all users");
        List<User> users = _userRepository.GetAllUsers();
        return _mapper.Map<List<UserResponse>>(users);
    }

    public UserResponse GetUserById(long id)
    {
        _logger.LogInformation("Fetching user with ID {UserId}", id);
        User user = _userRepository.GetUserById(id);
        return user != null ? _mapper.Map<UserResponse>(user) : null;
    }
}
