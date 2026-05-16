namespace RetailECommerce.Data;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;


public class DataSeeder
{
    public static async Task SeedAdminAsync(MyDbContext context)
    {
        // Ensure the database is created
        await context.Database.MigrateAsync();

        // Check if there are any users in the database
        if (!context.Users.Any())
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            // If not, create a default admin user
            var adminUser = new User
            {
                FullName = "Admin User",
                Email = "admin@example.com",
                Password = hashedPassword,      
                Role = UserRole.Vendor
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        } else
        {
            Console.WriteLine("Admin already exist in the database. Skipping seeding.");
        }
    }
}