
using EcommerceWebApp.BaseDBEntities;
using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace urbanMartAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDBContext _context;
        private readonly BaseDbContext _baseDbContext;

        public OrderRepository(EcommerceDBContext context, BaseDbContext baseDbContext)
        {
            _context = context;
            _baseDbContext = baseDbContext;
        }

        // Fetch all orders asynchronously
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Fetch orders by a specific user asynchronously
        public async Task<List<Order>> GetUserOrdersAsync(string username)
        {
            return await _context.Orders
                                  .Where(o => o.CreatedBy == username)
                                  .ToListAsync();
        }

        // Fetch a single order by its order ID asynchronously
        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _context.Orders
                                 .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        // Create a new order and save it asynchronously
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); // Save changes asynchronously
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Order was modified by another user.hhhhhhhhh");
            }
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .SumAsync(o => o.Price);
        }

        public async Task<List<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<List<CustomerSalesDto>> GetTopCustomersAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .GroupBy(o => o.CreatedBy)
                .Select(g => new CustomerSalesDto
                {
                    CustomerName = g.Key,
                    TotalSpent = g.Sum(o => o.Price),
                    OrdersCount = g.Count()
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(10)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageOrderValueAsync(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await GetTotalRevenueAsync(startDate, endDate);
            var orderCount = await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .CountAsync();

            return orderCount == 0 ? 0 : totalRevenue / orderCount;
        }


        //public async Task<IEnumerable<OrderItemData>> GetOrderItemDataAsync(DateTime startDate, DateTime endDate)
        //{
        //    var query = from o in _context.Orders
        //                join oi in _context.OrderItems on o.Id equals oi.OrderId
        //                join p in _context.Products on oi.ProductId equals p.Id
        //                join ca in _context.Carts on o.OrderId equals ca.Id into cartGroup
        //                from ca in cartGroup.DefaultIfEmpty()
        //                where o.OrderDate >= startDate && o.OrderDate <= endDate
        //                select new OrderItemData
        //                {
        //                    Order = o,
        //                    OrderItem = oi,
        //                    Product = p,
        //                    Cart = ca
        //                };

        //    return await query.ToListAsync();
        //}

    }
}

