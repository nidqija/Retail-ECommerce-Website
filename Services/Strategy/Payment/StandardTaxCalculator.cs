namespace RetailECommerce.Services.Strategy.Payment
{
    /// <summary>
    /// Standard implementation of tax calculation.
    /// Can be extended or replaced for different regions/tax rules.
    /// </summary>
    public class StandardTaxCalculator : ITaxCalculator
    {
        private readonly decimal _taxRate;

        /// <summary>
        /// Initialize with a specific tax rate.
        /// </summary>
        /// <param name="taxRate">Tax rate as decimal (e.g., 0.08 for 8%)</param>
        public StandardTaxCalculator(decimal taxRate = 0.08m)
        {
            _taxRate = taxRate;
        }

        public decimal CalculateTax(decimal subtotal)
        {
            if (subtotal <= 0)
                return 0;

            return Math.Round(subtotal * _taxRate, 2);
        }

        public decimal GetTaxRate()
        {
            return _taxRate;
        }
    }
}
