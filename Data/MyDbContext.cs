using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    // create a mapping between the notification and user model to create a relationship between them in the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId);


        modelBuilder.Entity<Enquiry>()
            .HasOne(e => e.User)
            .WithMany(u => u.Enquiries)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<Enquiry>()
            .HasOne(e => e.Product)
            .WithMany(p => p.Enquiries)
            .HasForeignKey(e => e.ProductId);

    }



    // declare product model to create a table in the database and perform crud operations on it
    // be sure to declare the other model here as well to create the tables in the db
    public DbSet<Product> Products { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Enquiry> Enquiries { get; set; }


    public DbSet<Review> Reviews { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

}
