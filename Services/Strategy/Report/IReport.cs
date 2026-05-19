using RetailECommerce.Models;

namespace RetailECommerce.Services.Strategy.Report;

// this interface is an Abstract Strategy
// it defines the contract for the report generation strategy
public interface IReportStrategy
{
    string reportType { get; }

    byte[] generateReport(ReportData reportData);




}