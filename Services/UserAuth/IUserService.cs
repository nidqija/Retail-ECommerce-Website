namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;


public interface IUserService
{
    Task <bool> RegisterUserAsync(User user);
    bool IsEmailUnique(string email);

    Task<User?> AuthenticateUserAsync(string email, string password);
    
}