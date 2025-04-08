using EcommerceWebApp.BaseDBEntities;

public interface IUserRepository
{
    List<User> GetAllUsers();
    User GetUserById(long id);
    User GetUserByUsername(string username);
    User GetUserByEmail(string email);
    User CreateUser(User user);

}
