using RetailECommerce.Models;
namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Enquiry
{
    [Key]
    public int EnquiryId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProductId { get; set; }

    // declare foreign key relationships to User  models
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    // declare foreign key relationships to Product models
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;


    [MaxLength(1000)]
    public string ReplyMessage { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status {get; set;} = "Pending";

    [Required]  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
