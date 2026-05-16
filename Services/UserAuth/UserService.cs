namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;


public class UserService : IUserService
{
    private readonly MyDbContext _context;
    
    
    public UserService(MyDbContext context) => _context = context;
 
    // registers a new user in the database
    // checks if the email is unique before registering the user
    public async Task<bool> RegisterUserAsync(User user , string password)
    {
        if (!IsEmailUnique(user.Email))
            return false;
        
        user.Password = BCrypt.HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    // checks if email is unqique by querying the database for any existing user with the same email
    public bool IsEmailUnique(string email)
    {
        return !_context.Users.Any(u => u.Email == email);
    }

    

    // retrieves a user from the database based on the provided email and password
    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null && BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        return null;
    }
   

    
}