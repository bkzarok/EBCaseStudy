using System.Collections.Generic;

namespace EBCaseStudy.Models.Api
{
    public class PaginatedProductResponse
    {
        public List<Product> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
