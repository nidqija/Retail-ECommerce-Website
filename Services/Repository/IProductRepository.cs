namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;


// ============= REPOSITORY PATTERN ============== //

// 1. this pattern is used to abstract the data access layer from the business logic layer.
// 2. it provides a way to manage the data access logic in another class , easier to maintain and test.
// 3. we can use this pattern to switch between different data sources (e.g. SQL Server, MongoDB, etc.) without changing the business logic layer.
// 4. business logic layer in this case is the controller and data access layer is the repository ( this class for example)



public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id); 
}
