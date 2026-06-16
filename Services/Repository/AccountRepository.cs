namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


public class AccountRepository : IAccountRepository
{
    private readonly MyDbContext _context;

    public AccountRepository(MyDbContext context)
    {
        _context = context;
    }

    public void ChangePassword( string newPassword , string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        
        if (user == null)
        {
            throw new Exception($"User with email {email} not found.");
        }

        // hash the new password before saving (placeholder logic)
        user.Password = HashPassword(newPassword);

        // saving password
        _context.SaveChanges();

        Console.WriteLine($"Password for user {email} has been changed to {newPassword} (hashed: {user.Password})"); 

    }

    private string HashPassword(string password)
    {
        // Placeholder for password hashing logic
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}