namespace RetailECommerce.Services.Facades;

using Microsoft.EntityFrameworkCore;
using RetailECommerce.Data;
using RetailECommerce.Models.DTOs;
using RetailECommerce.Models;
using RetailECommerce.Services.Repository;

public class AdminDashboardFacade
{
    private readonly MyDbContext _context;
    private readonly IProductRepository _productRepository;

    public AdminDashboardFacade(MyDbContext context, IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }

    public DashboardSummaryDTO GetDashboardSummary()
    {
        // 1. Calculate Total Revenue (Using Orders)
        var totalRevenue = _context.Orders
            .Where(o => o.OrderStatus != "Cancelled")
            .Sum(o => o.TotalAmount);

        // 2. Count Pending Orders
        var pendingOrders = _context.Orders
            .Count(o => o.OrderStatus == "Pending");

        // 3. Count Low Stock Alerts (e.g., threshold < 5)
        var lowStockCount = _productRepository.GetAllProducts()
            .Count(p => p.StockQuantity < 5);

        // 4. Get 5 most recent orders
        var recentOrders = _context.Orders
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToList();

        return new DashboardSummaryDTO
        {
            TotalRevenue = totalRevenue,
            PendingOrdersCount = pendingOrders,
            LowStockAlertsCount = lowStockCount,
            RecentOrders = recentOrders
        };
    }
}
