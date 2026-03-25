using EBCaseStudy.Models.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBCaseStudy.Services
{
    public interface IApiService
    {
        Task<PaginatedProductResponse> GetProductsAsync(int pageNumber = 1, int pageSize = 10, string? category = null, string? sortBy = null);
        Task<Product> GetProductAsync(int productId);
        Task<PaginatedPriceResponse> GetPriceHistoryAsync(int productId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
