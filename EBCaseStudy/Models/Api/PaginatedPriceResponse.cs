using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EBCaseStudy.Models.Api
{
    public class PaginatedPriceResponse
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
        [JsonPropertyName("items")]
        public List<Price> Items { get; set; }
    }
}
