namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Strategy.Report;





public class ReportController : Controller
{

   private readonly IReportStrategy _reportStrategy;

    public ReportController(IReportStrategy reportStrategy)
    {
        _reportStrategy = reportStrategy;
    
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
    public IActionResult GenerateReport(string reportType)
    {

        try
        {
           // generate the report using the strategy pattern
           byte[] reportData = _reportStrategy.generateReport(reportType);

            // return the file to the user device for auto download
            return File(reportData, "application/pdf", "ProductReport.pdf");

        } catch (ArgumentException ex)
        {
            // handle invalid report type error
            return BadRequest(ex.Message);
        }
        
    }






}