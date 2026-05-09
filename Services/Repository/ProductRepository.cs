namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;




// ============= REPOSITORY PATTERN ============== //
// 1. this pattern is used to abstract the data access layer from the business logic layer.
// 2. it provides a way to manage the data access logic in another class , easier to maintain and test.
// 3. we can use this pattern to switch between different data sources (e.g. SQL Server, MongoDB, etc.) without changing the business logic layer.
// 4. business logic layer in this case is the controller and data access layer is the repository ( this class for example)

// use this to access the data from the database and perform crud operations on the product model
// call this class in the controller to perform the operations on the product model and return the data to the client.


public class ProductRepository : IProductRepository
{
    private readonly MyDbContext _context;

    public ProductRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        var product = _context.Products.Find(id);
        
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        return product;
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}