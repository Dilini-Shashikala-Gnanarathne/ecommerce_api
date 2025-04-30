// /Services/SalesReportService.cs
using EcommerceWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceWebApp.Repositories.Implementations;
using urbanMartAPI.Repositories;

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


    }
}

//performance bottlenecks due to: Round trips to the database. Inefficient execution plans if not optimized individually

//Task.WhenAll() to run all the async calls in parallel


//public async Task<SalesReportDto> GetSalesPerformanceReport(DateTime startDate, DateTime endDate)
//{
//    // Start all tasks in parallel
//    var totalRevenueTask = _orderRepository.GetTotalRevenueAsync(startDate, endDate);
//    var topSellingProductsTask = _orderItemRepository.GetTopSellingProductsAsync(startDate, endDate);
//    var topCustomersTask = _orderRepository.GetTopCustomersAsync(startDate, endDate);
//    var abandonedCartsTask = _cartRepository.GetAbandonedCartsAsync(startDate, endDate);
//    var averageOrderValueTask = _orderRepository.GetAverageOrderValueAsync(startDate, endDate);
//    var cartAbandonmentRateTask = _cartRepository.GetCartAbandonmentRateAsync(startDate, endDate);

//    // Await all of them together
//    await Task.WhenAll(
//        totalRevenueTask,
//        topSellingProductsTask,
//        topCustomersTask,
//        abandonedCartsTask,
//        averageOrderValueTask,
//        cartAbandonmentRateTask
//    );

//    // Combine results into DTO
//    return new SalesReportDto
//    {
//        TotalRevenue = totalRevenueTask.Result,
//        TopSellingProducts = topSellingProductsTask.Result,
//        TopCustomers = topCustomersTask.Result,
//        AbandonedCarts = abandonedCartsTask.Result,
//        AverageOrderValue = averageOrderValueTask.Result,
//        CartAbandonmentRate = cartAbandonmentRateTask.Result
//    };
//}
