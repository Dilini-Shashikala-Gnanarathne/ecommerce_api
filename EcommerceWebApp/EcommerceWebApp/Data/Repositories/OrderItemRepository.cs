using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using EcommerceWebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApp.Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly EcommerceDBContext _context;

        public OrderItemRepository(EcommerceDBContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSalesDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.OrderItems
                .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt <= endDate)
                .GroupBy(oi => oi.ProductName)
                .Select(g => new ProductSalesDto
                {
                    ProductName = g.Key,
                    TotalSales = g.Sum(oi => oi.Quantity * oi.UnitPrice),
                    QuantitySold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.TotalSales)
                .Take(10)
                .ToListAsync();
        }
    }
}
