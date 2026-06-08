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

    // Receipt breakdown captured at checkout (so the order detail page can show
    // an accurate summary later, not just the grand total).
    public decimal Subtotal { get; set; }

    public decimal Tax { get; set; }

    // How the customer paid: "Credit / Debit Card", "QR Pay", "Cash on Delivery".
    public string? PaymentMethod { get; set; }

    public string OrderStatus { get; set; } = "Pending";
    
    public int? PaymentId { get; set; }
    [ForeignKey(nameof(PaymentId))]
    public Payment? Payment { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
