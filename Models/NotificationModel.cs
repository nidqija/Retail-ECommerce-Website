using RetailECommerce.Models;
namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;



public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [Required]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [MaxLength(250)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public NotificationType Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}