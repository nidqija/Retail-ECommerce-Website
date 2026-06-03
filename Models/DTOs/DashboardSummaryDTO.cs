namespace RetailECommerce.Models.DTOs;

public class DashboardSummaryDTO
{
    public decimal TotalRevenue { get; set; }
    public int PendingOrdersCount { get; set; }
    public int LowStockAlertsCount { get; set; }
    
    public List<Order> RecentOrders { get; set; } = new List<Order>(); 
}
