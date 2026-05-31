namespace RetailECommerce.Services.Strategy.Payment
{
    /// <summary>
    /// Segregated interface for tax calculation logic.
    /// Keeps tax concerns separate from payment execution.
    /// </summary>
    public interface ITaxCalculator
    {
        /// <summary>
        /// Calculate tax for a given subtotal.
        /// </summary>
        /// <param name="subtotal">The subtotal amount before tax</param>
        /// <returns>The calculated tax amount</returns>
        decimal CalculateTax(decimal subtotal);

        /// <summary>
        /// Get the tax rate as a percentage (e.g., 0.08 for 8%)
        /// </summary>
        decimal GetTaxRate();
    }
}
