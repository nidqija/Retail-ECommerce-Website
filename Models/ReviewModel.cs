namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



public class Review
{
    [Key]
    public int ReviewId {get; set;}

    [Required]
    public int UserId {get; set;}

    [Required]
    public int ProductId {get; set;}

     

    [Required]
    public int rating {get; set;}

    [Required]
    [MaxLength(1000)]
    public string Comment {get; set;} = string.Empty;


    [MaxLength(1000)]
    public string? VendorReply {get; set;}


    [MaxLength(1000)]
    public string? status {get; set;} = "Pending";


    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;


    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;


    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    





}