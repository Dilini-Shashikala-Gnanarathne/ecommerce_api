using EcommerceWebApp.BaseDBEntities;
using Microsoft.EntityFrameworkCore;


public class UserRepository : IUserRepository
{

    //private readonly DbContextFactory _contextFactory;

    //public UserRepository(DbContextFactory contextFactory)
    //{
    //    _contextFactory = contextFactory;
    //}

    //private DbContext GetContext(DbContextType contextType)
    //{
    //    return _contextFactory.GetContext(contextType);
    //}

    //public async Task<List<User>> GetAllUsers(DbContextType contextType)
    //{
    //    var context = GetContext(contextType);
    //    return await context.Set<User>().ToListAsync();
    //}
    private readonly BaseDbContext _context;

    private readonly EcommerceDbContext _ecomcontext;

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
