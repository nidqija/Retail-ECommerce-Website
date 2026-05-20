namespace RetailECommerce.Services.Factory.Report;
using RetailECommerce.Models;


// abstract product
// this is used to set a contract to render different report data types
public interface IReportData
{
    string TargetReportDataType { get; }


    ReportData MapData();
}