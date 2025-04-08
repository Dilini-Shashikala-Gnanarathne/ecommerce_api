// /Repositories/Interfaces/IOrderItemRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<List<ProductSalesDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate);
    }
}
