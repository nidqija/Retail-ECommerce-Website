namespace RetailECommerce.Services.Factory.Report;
using RetailECommerce.Models;


// abstract product
// this is used to set a contract to render different report data types
public interface IReportData
{
    string TargetReportDataType { get; }

    // open a new parameter to pass any parameter needed for the report data mapping, such as month for sales report
    ReportData MapData(Dictionary<string, string>? parameters = null);
}