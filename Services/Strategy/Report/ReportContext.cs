namespace RetailECommerce.Services.Strategy.Report;

// this is the context class for the strategy pattern
// it is used to call the report generation method from the strategy class
public class ReportContext
{
    private readonly IReportStrategy _reportStrategy;

    public ReportContext(IReportStrategy reportStrategy)
    {
        _reportStrategy = reportStrategy;
    }

    public void GenerateReport(string reportType)
    {
        _reportStrategy.generateReport(reportType);
    }
}