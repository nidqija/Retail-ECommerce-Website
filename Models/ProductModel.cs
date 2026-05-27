namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Product
{
    
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = "General";

    [MaxLength(50)]
    public string SubCategory { get; set; } = "General";
}