using System;

namespace EBCaseStudy.Models.Api
{
    public class Price
    {
        public int ProductId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
