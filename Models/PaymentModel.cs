namespace RetailECommerce.Models;
using RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public decimal Total_Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    public PaymentStatus PaymentStatus { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }
}
