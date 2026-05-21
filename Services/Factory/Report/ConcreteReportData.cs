
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

    public ReportData MapData(Dictionary<string, string>? parameters = null)
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

    public ReportData MapData(Dictionary<string, string>? parameters = null)
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



public class PaymentReportData : IReportData
{
    private readonly MyDbContext _context;
    public string TargetReportDataType => "PaymentReport";

    public PaymentReportData(MyDbContext context)
    {
        _context = context;
    }

    public ReportData MapData(Dictionary<string, string>? parameters = null)
    {
        int currentMonth = DateTime.Now.Month;

        if (parameters != null && parameters.ContainsKey("Month"))
        {
            if (int.TryParse(parameters["Month"], out int month))
            {
                currentMonth = month;
            }
        }

        int targetYear = DateTime.Now.Year;

        var startofDate = new DateTime(targetYear, currentMonth, 1);
        var endOfDate = startofDate.AddMonths(1).AddDays(-1);


        var reportData = new ReportData
        {
            Title = "Payment Report for the month of " + new DateTime(targetYear, currentMonth, 1).ToString("MMMM"),
            Headers = new List<string> { "User Email", "Amount", "Status" },
            Rows = _context.Payments.Where(p => p.PaymentDate >= startofDate && p.PaymentDate <= endOfDate).Select(p => new List<string>
            {
                p.User.Email,
                $"${p.Total_Amount}",
                p.PaymentStatus.ToString()
            }).ToList()
         };

         return reportData;
        }

}


