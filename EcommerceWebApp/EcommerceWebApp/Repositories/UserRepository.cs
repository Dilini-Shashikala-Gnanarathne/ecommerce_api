using EcommerceWebApp.BaseDBEntities;
using EcommerceWebApp.EcommerceDBEntities;
using Microsoft.EntityFrameworkCore;


public class UserRepository : IUserRepository
{
    private readonly BaseDbContext _context;

    private readonly EcommerceDBContext _ecomcontext;

    public UserRepository(BaseDbContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(long id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
}
