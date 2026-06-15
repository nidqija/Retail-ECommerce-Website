namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Factory.Report;
using RetailECommerce.Services.Strategy.Report;





public class ReportController : Controller
{

   private readonly IEnumerable<IReportStrategy> _reportStrategies;
   private readonly MyDbContext _context;

    public ReportController(IEnumerable<IReportStrategy> reportStrategies, MyDbContext context)
    {
        _reportStrategies = reportStrategies;
        _context = context;
    }


   public IActionResult Index()
    {
        PageCreator pageCreator = new ReportPageCreator();
        return pageCreator.RenderPage(this);
    }


    [HttpGet]
    // iactionresult is used to generate report based on the strategy pattern
    // this method is from microsoft asp.net core mvc framework
    // it is used to handle the post request from the report page

    
    public IActionResult GenerateReport(string reportType , string dataType , string month)
    {

        try
        {
           if (string.IsNullOrEmpty(reportType))
            {
                return BadRequest("Report type is required.");
            } 

            ReportFactory reportFactory = new ReportFactory(_context);
            IReportData reportData = reportFactory.GetReportData(dataType);


            var reportParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(month))
            {
                reportParameters.Add("Month", month);
            }
          
            ReportData data = reportData.MapData(reportParameters);
            var activeStrategy = _reportStrategies.FirstOrDefault(s => s.reportType.Equals(reportType, StringComparison.OrdinalIgnoreCase));
            

            if (activeStrategy == null)
            {
                return BadRequest("Invalid report type.");
            }

           // client 
            ReportContext reportContext = new ReportContext(activeStrategy);

            byte[] reportBytes = reportContext.GenerateReport(data);   

            GetContentType(reportType);          

            return File(reportBytes, GetContentType(reportType), $"{reportType}_Report.{GetFileExtension(reportType)}");
            
        } catch (ArgumentException ex)
        {
            // handle invalid report type error
            return BadRequest(ex.Message);
        }
        
    }

    private string GetContentType(string reportType)
    {
        return reportType switch
        {
            "PDF" => "application/pdf",
            "CSV" => "text/csv",
            _ => "application/octet-stream"
        };
    }

    private string GetFileExtension(string reportType)
    {
        return reportType switch
        {
            "PDF" => "pdf",
            "CSV" => "csv",
            _ => "dat"
        };
    }






}