namespace RetailECommerce.Services.Strategy.Report;
using Microsoft.EntityFrameworkCore;
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
    }
    

    // separate the method to get the product data from the database and questpdf to keep the code clean and maintainable
    public List<string> GetProductReportData()
    {
        return _context.Products.Select(p => p.Name).ToList();
    }

    // this method generates a pdf report based on the product data in the database using the questpdf library
    public byte[] generateReport(string reportType)
    {
        if (reportType != "PDF")
        {
            throw new ArgumentException("Invalid report type.");
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
                            // create columns
                            columns.RelativeColumn(2); 
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Cell().Text("Product Name").FontSize(14).Bold();
                        table.Cell().Text("Price").FontSize(14).Bold();
                        table.Cell().Text("Stock Quantity").FontSize(14).Bold();


                        foreach (var product in _context.Products.ToList())
                        {
                            table.Cell().Text(product.Name);
                            table.Cell().Text($"${product.Price}");
                            table.Cell().Text(product.StockQuantity.ToString());
                        }
                    });
                    
                });
            }).GeneratePdf();
            
        }
    }


}


