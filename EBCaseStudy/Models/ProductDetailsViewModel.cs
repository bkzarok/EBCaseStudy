using EBCaseStudy.Models.Api;
using System.Collections.Generic;

namespace EBCaseStudy.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Price> PriceHistory { get; set; }
    }
}
