using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    // declare product model to create a table in the database and perform crud operations on it
    // be sure to declare the other model here as well to create the tables in the db
    public DbSet<Product> Products { get; set; }
}
