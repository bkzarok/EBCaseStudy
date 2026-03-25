using EBCaseStudy.Models.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace EBCaseStudy.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.casestudy.enterbridge.com/");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"/api/products/{productId}", _jsonSerializerOptions);
        }

        public async Task<PaginatedProductResponse> GetProductsAsync(int pageNumber = 1, int pageSize = 10, string category = null, string sortBy = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pageNumber"] = pageNumber.ToString();
            query["pageSize"] = pageSize.ToString();
            if (!string.IsNullOrEmpty(category))
            {
                query["category"] = category;
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                query["sortBy"] = sortBy;
            }

            var response = await _httpClient.GetAsync($"/api/products?{query}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PaginatedProductResponse>(content, _jsonSerializerOptions);
        }

        public async Task<List<Price>> GetPriceHistoryAsync(int productId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["productId"] = productId.ToString();
            if (startDate.HasValue)
            {
                query["startDate"] = startDate.Value.ToString("yyyy-MM-dd");
            }
            if (endDate.HasValue)
            {
                query["endDate"] = endDate.Value.ToString("yyyy-MM-dd");
            }
            
            var response = await _httpClient.GetAsync($"/api/prices?{query}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<Price>();
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<Price>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<Price>>(content, _jsonSerializerOptions) ?? new List<Price>();
            }
            catch (JsonException)
            {
                try
                {
                    var singlePrice = JsonSerializer.Deserialize<Price>(content, _jsonSerializerOptions);
                    if (singlePrice != null)
                    {
                        return new List<Price> { singlePrice };
                    }
                }
                catch (JsonException)
                {
                    // Ignore and return empty list if it's not a single price object either
                }
            }

            return new List<Price>();
        }
    }
}
