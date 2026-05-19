namespace RetailECommerce.Models;

public class ReportData
{
    public string Title { get; set; } = "System Report";
    public List<string> Headers { get; set; } = new ();
    public List<List<string>> Rows { get; set; } = new ();

}