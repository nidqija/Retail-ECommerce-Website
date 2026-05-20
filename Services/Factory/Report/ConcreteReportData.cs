
namespace RetailECommerce.Models;
using RetailECommerce.Services.Factory.Report;
using System.Linq;

// concrete product for report data
// this class is responsible for mapping the data from the database to the report data model
public class ProductReportData : IReportData
{
    private readonly MyDbContext _context;
    public string TargetReportDataType => "ProductReport";

    public ProductReportData(MyDbContext context)
    {
        _context = context;
    }

    public ReportData MapData()
    {
        var reportData = new ReportData
        {
            Title = "Product Report",
            Headers = new List<string> { "Name", "Price", "Stock Quantity" },
            Rows = _context.Products.Select(p => new List<string>
            {
                p.Name,
                $"${p.Price}",
                p.StockQuantity.ToString()
            }).ToList()
         };

         return reportData;
        }

        
    }


// concrete product for user report data
// this class is responsible for mapping the data from the database to the report data model
public class UserReportData : IReportData
{
    private readonly MyDbContext _context;
    public string TargetReportDataType => "UserReport";

    public UserReportData(MyDbContext context)
    {
        _context = context;
    }

    public ReportData MapData()
    {
        var reportData = new ReportData
        {
            Title = "User Report",
            Headers = new List<string> { "Email", "Full Name" },
            Rows = _context.Users.Select(u => new List<string>
            {
                u.Email,
                u.FullName
            }).ToList()
         };

         return reportData;
        }
}


