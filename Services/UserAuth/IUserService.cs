namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;


public interface IUserService
{
    Task <bool> RegisterUserAsync(User user , string password);
    bool IsEmailUnique(string email);

    Task<User?> AuthenticateUserAsync(string email, string password);
    
}