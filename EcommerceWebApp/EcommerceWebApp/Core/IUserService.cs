public interface IUserService
{
    List<UserResponse> GetAllUsers();
    UserResponse GetUserById(long id);
}