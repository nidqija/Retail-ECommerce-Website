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


    public static async Task SeedProductAsync(MyDbContext context)
    {
        // Ensure the database is created
        await context.Database.MigrateAsync();

        // Check if there are any products in the database
        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Name = "Red Shirt", Description = "A comfortable red shirt", Price = 19.99m, StockQuantity = 10 },
                new Product { Name = "Blue Jeans", Description = "Stylish blue jeans", Price = 49.99m, StockQuantity = 20 },
                new Product { Name = "Black Shoes", Description = "Elegant black shoes", Price = 79.99m, StockQuantity = 15 },
                new Product { Name = "White Hat", Description = "Simple white hat", Price = 14.99m, StockQuantity = 12 }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        } else
        {
            Console.WriteLine("Products already exist in the database. Skipping seeding.");
        }
    }


    public static async Task SeedPaymentAsync(MyDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Payments.Any())
        {
            var payments = new List<Payment>
            {
                new Payment { Total_Amount = 99.99m, PaymentDate = DateTime.Now, PaymentStatus = PaymentStatus.Completed, PaymentMethod = PaymentMethod.CreditCard , UserId = 1},  
                new Payment { Total_Amount = 49.99m, PaymentDate = DateTime.Now.AddMonths(-1), PaymentStatus = PaymentStatus.Pending, PaymentMethod = PaymentMethod.PayPal, UserId = 1 },
                new Payment { Total_Amount = 19.99m, PaymentDate = DateTime.Now.AddMonths(-2), PaymentStatus = PaymentStatus.Failed, PaymentMethod = PaymentMethod.BankTransfer, UserId = 1 }
            };

            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();
        } else
        {
            Console.WriteLine("Payments already exist in the database. Skipping seeding.");
        }
    }

    
}