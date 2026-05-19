namespace RetailECommerce.Services.Strategy.Report;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;





public class CSVReportStrategy : IReportStrategy
{
    private readonly MyDbContext _context;
    public string reportType => "CSV";

    public CSVReportStrategy(MyDbContext context)
    {
        _context = context;

    }

    public byte[] generateReport(ReportData reportData)
    {
        

        Console.Write(reportData.Title + "\n");

        foreach (var headers in reportData.Headers)
        {
            Console.Write(headers + ",");
        }

        foreach (var row in reportData.Rows)
        {
            Console.WriteLine();
            foreach (var cell in row)
            {
                Console.Write(cell + ",");
            }
        }


        string csvContent = "Title, " + reportData.Title + "\n" +
                            string.Join(",", reportData.Headers) + "\n" +
                            string.Join("\n", reportData.Rows.Select(row => string.Join(",", row)));


        return System.Text.Encoding.UTF8.GetBytes(csvContent);
    }

    

    
}