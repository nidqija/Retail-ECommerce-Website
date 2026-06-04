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
    [StringLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string DiscountCode { get; set; } = null!;


    [Required]
    [Range(0, 100)]
    public decimal DiscountPercentage { get; set; }


    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;



    
}