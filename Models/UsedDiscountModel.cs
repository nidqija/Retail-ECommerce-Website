namespace RetailECommerce.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Records that a specific user has already redeemed a specific discount.
/// One row per (user, discount) usage. Used at checkout to disable discount
/// codes the user has already used so they can't be applied again.
/// </summary>
public class UsedDiscount
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int DiscountId { get; set; }

    [Required]
    public DateTime UsedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [ForeignKey(nameof(DiscountId))]
    public Discount? Discount { get; set; }
}
