namespace RetailECommerce.Services.Factory.Report;
using RetailECommerce.Models;


// product factory
// this class is used to create instances of the report data classes based on the report data type requested
// it will then serves the report data to the report generation strategy to generate the report according to the requested format
public class ReportFactory
{
    private readonly MyDbContext _context;

    public ReportFactory(MyDbContext context)
    {
        _context = context;
    }

    public IReportData GetReportData(string reportDataType)
    {
        return reportDataType switch
        {
            "ProductReport" => new ProductReportData(_context),
            "UserReport" => new UserReportData(_context),
            "PaymentReport" => new PaymentReportData(_context),
            "OrderReport" => new OrderReportData(_context),
            _ => throw new ArgumentException("Invalid report data type")
        };
    }
}