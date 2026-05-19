namespace RetailECommerce.Services.Strategy.Report;
using RetailECommerce.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;


// Concrete Strategy for PDF report
public class PDFReportStrategy : IReportStrategy
{
    private readonly MyDbContext _context;
    
    public string reportType => "PDF";

    public PDFReportStrategy(MyDbContext context)
    {
        _context = context;
        QuestPDF.Settings.License = LicenseType.Community;
    }
    

    

    

    // this method generates a pdf report based on the product data in the database using the questpdf library
    public byte[] generateReport(ReportData reportData)
    {
        if (this.reportType != "PDF")
        {
            throw new InvalidOperationException("Invalid report type for PDFReportStrategy");
        } else
        {
         return Document.Create(container =>
            {
                container.Page(page =>
                {
                    // decide the page size and margin
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().Text("Live Product Inventory Report").FontSize(22).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var header in reportData.Headers)
                            {
                                columns.RelativeColumn();
                            }
                        });

                       table.Header(header =>
                    {
                        foreach (var headerText in reportData.Headers)
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(headerText).FontSize(14).Bold();
                        }
                    });

                    foreach (var row in reportData.Rows)
                        {
                            foreach (var cell in row)
                            {
                                
                                string cleanContent = cell ?? string.Empty;
                                
                                table.Cell()
                                     .BorderBottom(0.5f)
                                     .BorderColor(Colors.Grey.Lighten1)
                                     .Padding(5)
                                     .Text(cleanContent)
                                     .FontSize(12);
                            }
                        }
                  });
                    
                });
            }).GeneratePdf();
            
   

        }
    }
}


