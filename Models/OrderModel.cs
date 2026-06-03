namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    public decimal TotalAmount { get; set; }
    
    public string OrderStatus { get; set; } = "Pending"; 
    
    public int? PaymentId { get; set; }
    [ForeignKey(nameof(PaymentId))]
    public Payment? Payment { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
