
using EcommerceWebApp.BaseDBEntities;
using EcommerceWebApp.EcommerceDBEntities;
using EcommerceWebApp.Models;
using EcommerceWebApp.Models.EcommerceWebApp.Dtos;
using Microsoft.Data.SqlClient;
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

        //---------------- Using LINQ with Joins ------------
        /**
         * Pros: Easy to read and maintain. Works well within the app, and don’t need extra setup.
         * Cons: Can be slow for complex queries, especially with large data. Might use more memory if not optimized.
         */
        //public async Task<List<OrderReportDto>> GetOrderReportAsync(string status, DateTime startDate)
        //{
        //    var query = from o in _context.Orders
        //                join oi in _context.OrderItems on o.Id equals oi.OrderId
        //                join p in _context.Products on oi.ProductId equals p.Id
        //                where o.Status == status && o.CreatedAt >= startDate
        //                orderby o.CreatedAt descending
        //                select new OrderReportDto
        //                {
        //                    OrderId = o.OrderId,
        //                    OrderName = o.OrderName,
        //                    OrderTotalPrice = o.Price,
        //                    OrderStatus = o.Status,
        //                    OrderCreatedAt = o.CreatedAt,
        //                    OrderCreatedBy = o.CreatedBy,
        //                    OrderUserNic = o.UserNic,
        //                    ProductId = oi.ProductId,
        //                    OrderItemProductName = oi.ProductName,
        //                    OrderItemQuantity = oi.Quantity,
        //                    OrderItemUnitPrice = oi.UnitPrice,
        //                    OrderItemTotalPrice = oi.Quantity * oi.UnitPrice,
        //                    ProductName = p.Name,
        //                    ProductPrice = p.Price,

        //                };

        //    return await query.ToListAsync();
        //}

        //----------------  LINQ with AsNoTracking() ------------
        /**
         * Pros: Improves performance by not tracking changes, which makes it faster and uses less memory.
         * Cons: You can’t modify the data once it’s fetched, and it might still be slow with large data.
         */
        //public async Task<List<OrderReportDto>> GetOrderReportAsync(string status, DateTime startDate)
        //{
        //    // Apply AsNoTracking to prevent change tracking and improve performance for read-only queries
        //    var query = from o in _context.Orders.AsNoTracking() // Use AsNoTracking to avoid tracking
        //                join oi in _context.OrderItems.AsNoTracking() on o.Id equals oi.OrderId // Apply AsNoTracking on the join too
        //                join p in _context.Products.AsNoTracking() on oi.ProductId equals p.Id // Apply AsNoTracking on products
        //                where o.Status == status && o.CreatedAt >= startDate
        //                orderby o.CreatedAt descending
        //                select new OrderReportDto
        //                {
        //                    OrderId = o.OrderId,
        //                    OrderName = o.OrderName,
        //                    OrderTotalPrice = o.Price,
        //                    OrderStatus = o.Status,
        //                    OrderCreatedAt = o.CreatedAt,
        //                    OrderCreatedBy = o.CreatedBy,
        //                    OrderUserNic = o.UserNic,
        //                    ProductId = oi.ProductId,
        //                    OrderItemProductName = oi.ProductName,
        //                    OrderItemQuantity = oi.Quantity,
        //                    OrderItemUnitPrice = oi.UnitPrice,
        //                    OrderItemTotalPrice = oi.Quantity * oi.UnitPrice,
        //                    ProductName = p.Name,
        //                    ProductPrice = p.Price,
        //                };

        //    // Using ToListAsync to execute the query asynchronously and return the result
        //    return await query.ToListAsync();
        //}

        //----------------  Stored Procedures------------
        /**
         * Pros: Very fast since the query runs directly on the database, reducing data transfer and processing time.
         * Cons: Tied to a specific database, harder to change or debug, and requires managing database code separately.
         */
        public async Task<List<OrderReportDto>> GetOrderReportAsync(string status, DateTime startDate)
        {
            var statusParam = new SqlParameter("@Status", status);
            var startDateParam = new SqlParameter("@StartDate", startDate);

            var result = await _context.Set<OrderReportDto>().FromSqlRaw(
                "EXEC GetOrderReport @Status, @StartDate",
                statusParam,
                startDateParam)
                .ToListAsync();

            return result;
        }

    }

}

