namespace RetailECommerce.Models;
using System.ComponentModel.DataAnnotations;

public enum PaymentStatus
{
    [Display(Name = "Pending")]
    Pending,

    [Display(Name = "Completed")]
    Completed,

    [Display(Name = "Failed")]
    Failed,

    [Display(Name = "Refunded")]
    Refunded
}