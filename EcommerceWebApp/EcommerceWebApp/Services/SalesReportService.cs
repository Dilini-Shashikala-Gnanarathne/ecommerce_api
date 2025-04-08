// /Services/SalesReportService.cs
using EcommerceWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebApp.Services
{
    public class SalesReportService : ISalesReportService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICartRepository _cartRepository;

        public SalesReportService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartRepository = cartRepository;
        }

        public async Task<SalesReportDto> GetSalesPerformanceReport(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await _orderRepository.GetTotalRevenueAsync(startDate, endDate);
            var topSellingProducts = await _orderItemRepository.GetTopSellingProductsAsync(startDate, endDate);
            var topCustomers = await _orderRepository.GetTopCustomersAsync(startDate, endDate);
            var abandonedCarts = await _cartRepository.GetAbandonedCartsAsync(startDate, endDate);
            var averageOrderValue = await _orderRepository.GetAverageOrderValueAsync(startDate, endDate);
            var cartAbandonmentRate = await _cartRepository.GetCartAbandonmentRateAsync(startDate, endDate);

            return new SalesReportDto
            {
                TotalRevenue = totalRevenue,
                TopSellingProducts = topSellingProducts,
                TopCustomers = topCustomers,
                AbandonedCarts = abandonedCarts,
                AverageOrderValue = averageOrderValue,
                CartAbandonmentRate = cartAbandonmentRate
            };
        }

        //public async Task<SalesReportDto> GetSalesPerformanceReport(DateTime startDate, DateTime endDate)
        //{
        //    var query = from o in _context.Orders
        //                join oi in _context.OrderItems on o.Id equals oi.OrderId
        //                join p in _context.Products on oi.ProductId equals p.Id
        //                join c in _context.Customers on o.CustomerId equals c.Id
        //                join ca in _context.Carts on o.CartId equals ca.Id into cartGroup
        //                from ca in cartGroup.DefaultIfEmpty()
        //                where o.OrderDate >= startDate && o.OrderDate <= endDate
        //                select new
        //                {
        //                    Order = o,
        //                    OrderItem = oi,
        //                    Product = p,
        //                    Customer = c,
        //                    Cart = ca
        //                };

        //    var results = await query.ToListAsync();

        //    // Calculate aggregates
        //    var totalRevenue = results.Sum(r => r.OrderItem.Price * r.OrderItem.Quantity);
        //    var topSellingProducts = results.GroupBy(r => r.Product.Name)
        //                                    .OrderByDescending(g => g.Count())
        //                                    .Select(g => g.Key)
        //                                    .Take(10); // Example: Top 10 products
        //    var topCustomers = results.GroupBy(r => r.Customer.Name)
        //                               .OrderByDescending(g => g.Count())
        //                               .Select(g => g.Key)
        //                               .Take(10); // Example: Top 10 customers
        //    var abandonedCarts = await _cartRepository.GetAbandonedCartsAsync(startDate, endDate); // This might still need a separate query
        //    var averageOrderValue = results.Average(r => r.Order.TotalPrice);
        //    var cartAbandonmentRate = await _cartRepository.GetCartAbandonmentRateAsync(startDate, endDate); // This might still need a separate query

        //    return new SalesReportDto
        //    {
        //        TotalRevenue = totalRevenue,
        //        TopSellingProducts = topSellingProducts.ToList(),
        //        TopCustomers = topCustomers.ToList(),
        //        AbandonedCarts = abandonedCarts,
        //        AverageOrderValue = averageOrderValue,
        //        CartAbandonmentRate = cartAbandonmentRate
        //    };
        //}

        //public async Task<SalesReportDto> GetSalesPerformanceReport(DateTime startDate, DateTime endDate)
        //{
        //    var salesData = await _orderRepository.GetOrderItemDataAsync(startDate, endDate);

        //    // Calculate aggregates
        //    var totalRevenue = salesData.Sum(r => r.OrderItem.Price * r.OrderItem.Quantity);

        //    var topSellingProducts = salesData.GroupBy(r => r.Product.Name)
        //                                       .OrderByDescending(g => g.Sum(x => x.OrderItem.Quantity))
        //                                       .Select(g => g.Key)
        //                                       .Take(10) // Example: Top 10 products
        //                                       .ToList();

        //    var topCustomers = salesData.GroupBy(r => r.Customer.Name)
        //                                 .Select(g => new CustomerSalesDto
        //                                 {
        //                                     CustomerName = g.Key,
        //                                     TotalSpent = g.Sum(x => x.Order.Price),
        //                                     OrdersCount = g.Count()
        //                                 })
        //                                 .OrderByDescending(c => c.TotalSpent)
        //                                 .Take(10) // Example: Top 10 customers
        //                                 .ToList();

        //    var abandonedCartsCount = salesData.Count(r => r.Cart != null && r.Cart.Status == "Abandoned");

        //    var averageOrderValue = salesData.Any() ? totalRevenue / salesData.Select(r => r.Order).Distinct().Count() : 0; // Ensure distinct orders

        //    var cartAbandonmentRate = salesData.Any() ? (double)abandonedCartsCount / salesData.Count() : 0;

        //    return new SalesReportDto
        //    {
        //        TotalRevenue = totalRevenue,
        //        TopSellingProducts = topSellingProducts,
        //        TopCustomers = topCustomers,
        //        AbandonedCarts = abandonedCartsCount,
        //        AverageOrderValue = averageOrderValue,
        //        CartAbandonmentRate = cartAbandonmentRate
        //    };
        //}

    }
}
