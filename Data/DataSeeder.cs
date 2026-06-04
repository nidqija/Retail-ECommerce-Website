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
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
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


    public static async Task SeedEnquiryAsync(MyDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Enquiries.Any())
        {
            var enquiries = new List<Enquiry>
            {
                new Enquiry { UserId = 1, ProductId = 1, Message = "Is this product available in size M?", Status = "Pending", ReplyMessage = "", CreatedAt = DateTime.Now },
                new Enquiry { UserId = 1, ProductId = 2, Message = "What is the warranty period for this product?", Status = "Pending", ReplyMessage = "", CreatedAt = DateTime.Now.AddDays(-1) },
                new Enquiry { UserId = 1, ProductId = 3, Message = "Can I return this product if it doesn't fit?", Status = "Pending", ReplyMessage = "", CreatedAt = DateTime.Now.AddDays(-2) }
            };

            context.Enquiries.AddRange(enquiries);
            await context.SaveChangesAsync();
        } else
        {
            Console.WriteLine("Enquiries already exist in the database. Skipping seeding.");
        }
    }


    public static async Task SeedReviewAsync(MyDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Reviews.Any())
        {
            var reviews = new List<Review>
            {
                new Review { UserId = 1, ProductId = 1, Rating = 5, Comment = "Great product!", VendorReply = "", CreatedAt = DateTime.UtcNow },
                new Review { UserId = 1, ProductId = 2, Rating = 4, Comment = "Good quality.", VendorReply = "", CreatedAt = DateTime.UtcNow },
                new Review { UserId = 1, ProductId = 3, Rating = 3, Comment = "Average product.", VendorReply = "", CreatedAt = DateTime.UtcNow }
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        } else
        {
            Console.WriteLine("Reviews already exist in the database. Skipping seeding.");
        }
    }

    public static async Task SeedOrderAsync(MyDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order 
                { 
                    UserId = 1, 
                    OrderDate = DateTime.Now.AddDays(-1), 
                    TotalAmount = 89.97m, 
                    OrderStatus = "Completed",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 19.99m },
                        new OrderItem { ProductId = 2, Quantity = 1, UnitPrice = 49.99m }
                    }
                },
                new Order 
                { 
                    UserId = 1, 
                    OrderDate = DateTime.Now, 
                    TotalAmount = 79.99m, 
                    OrderStatus = "Pending",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ProductId = 3, Quantity = 1, UnitPrice = 79.99m }
                    }
                }
            };

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        } 
        else
        {
            Console.WriteLine("Orders already exist in the database. Skipping seeding.");
        }
    }
}