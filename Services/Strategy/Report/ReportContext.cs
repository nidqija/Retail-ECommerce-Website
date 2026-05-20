namespace RetailECommerce.Services.Strategy.Report;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;

// this is the context class for the strategy pattern
// it is used to call the report generation method from the strategy class
public class ReportContext
{
    private readonly IReportStrategy _reportStrategy;

    public ReportContext(IReportStrategy reportStrategy)
    {
        _reportStrategy = reportStrategy;
    }

    public byte[] GenerateReport(ReportData reportData)
    {
        return _reportStrategy.generateReport(reportData);
    }
}