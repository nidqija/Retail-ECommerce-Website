using RetailECommerce.Models;

namespace RetailECommerce.Services.Discounts
{
    /// <summary>
    /// Outcome of attempting to apply a discount code to a subtotal.
    /// Carries both the calculated figures and a user-facing message so the
    /// controller/view can explain why a code was (or was not) applied.
    /// </summary>
    public class DiscountResult
    {
        public bool IsApplied { get; set; }
        public string Message { get; set; } = string.Empty;

        public decimal OriginalSubtotal { get; set; }
        public decimal DiscountAmount { get; set; }

        // The amount the customer actually pays before tax.
        public decimal DiscountedSubtotal => OriginalSubtotal - DiscountAmount;

        // The matched discount, when one was applied.
        public Discount? Discount { get; set; }

        /// <summary>
        /// No code supplied - charge the full subtotal.
        /// </summary>
        public static DiscountResult None(decimal subtotal) => new()
        {
            IsApplied = false,
            Message = "No discount applied.",
            OriginalSubtotal = subtotal,
            DiscountAmount = 0m
        };

        /// <summary>
        /// A code was supplied but could not be honoured (unknown/expired).
        /// The full subtotal still stands.
        /// </summary>
        public static DiscountResult Rejected(decimal subtotal, string reason) => new()
        {
            IsApplied = false,
            Message = reason,
            OriginalSubtotal = subtotal,
            DiscountAmount = 0m
        };
    }
}
