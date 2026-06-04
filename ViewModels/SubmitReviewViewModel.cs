namespace RetailECommerce.ViewModels;

using System.ComponentModel.DataAnnotations;

public class SubmitReviewViewModel
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
    public int Rating { get; set; }

    [MaxLength(500, ErrorMessage = "Comment cannot be more than 500 characters.")]
    public string Comment { get; set; } = string.Empty;
}