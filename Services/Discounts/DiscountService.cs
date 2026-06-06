using System;
using System.Collections.Generic;
using RetailECommerce.Models;
using RetailECommerce.Services.Repository;

namespace RetailECommerce.Services.Discounts
{
    /// <summary>
    /// Default discount logic. Resolves codes through the repository and applies
    /// a straight percentage cut-off to the subtotal, while enforcing the
    /// discount's start/end (expiry) dates.
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public IEnumerable<Discount> GetAvailableDiscounts()
        {
            return _discountRepository.GetActiveDiscounts();
        }

        public DiscountResult ApplyDiscount(string? code, decimal subtotal)
        {
            // Nothing chosen - charge the full amount.
            if (string.IsNullOrWhiteSpace(code))
            {
                return DiscountResult.None(subtotal);
            }

            var discount = _discountRepository.GetDiscountByCode(code);
            if (discount == null)
            {
                return DiscountResult.Rejected(subtotal, $"Discount code \"{code}\" is not valid.");
            }

            // Enforce the expiry window. IsActive == (Now between StartDate and EndDate).
            if (!discount.IsActive)
            {
                var now = DateTime.Now;
                var reason = now < discount.StartDate
                    ? $"Discount \"{discount.DiscountCode}\" is not active yet (starts {discount.StartDate:MMM dd, yyyy})."
                    : $"Discount \"{discount.DiscountCode}\" has expired (ended {discount.EndDate:MMM dd, yyyy}).";
                return DiscountResult.Rejected(subtotal, reason);
            }

            // Percentage cut-off on the subtotal, rounded to cents.
            var discountAmount = Math.Round(subtotal * (discount.DiscountPercentage / 100m), 2);

            // Guard against a discount ever exceeding the subtotal.
            if (discountAmount > subtotal)
            {
                discountAmount = subtotal;
            }

            return new DiscountResult
            {
                IsApplied = true,
                Discount = discount,
                OriginalSubtotal = subtotal,
                DiscountAmount = discountAmount,
                Message = $"{discount.DiscountName} ({discount.DiscountPercentage:0.##}% off) applied."
            };
        }
    }
}
