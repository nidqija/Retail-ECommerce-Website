using System.Collections.Generic;
using RetailECommerce.Models;

namespace RetailECommerce.Services.Discounts
{
    /// <summary>
    /// Encapsulates the discount-code business rules used at checkout:
    /// which codes are offered, and how a chosen code affects the subtotal.
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Discounts the customer may pick from on the order page
        /// (only those currently within their validity window).
        /// </summary>
        IEnumerable<Discount> GetAvailableDiscounts();

        /// <summary>
        /// Validate the chosen code and calculate the cut-off on the subtotal.
        /// Honours the discount's expiry dates - expired or not-yet-started
        /// codes are rejected and the full subtotal is returned.
        /// </summary>
        /// <param name="code">The discount code chosen by the user (may be null/empty).</param>
        /// <param name="subtotal">The order subtotal before tax.</param>
        DiscountResult ApplyDiscount(string? code, decimal subtotal);
    }
}
