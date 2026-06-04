namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



public class Discount
{
    [Key]
    [Required]
    public int Id { get; set; }


    [Required]
    [StringLength(100)]
    public string DiscountName { get; set; } = null!;


    [Required]
    [Range(0, 100)]
    public decimal DiscountPercentage { get; set; }

    
}