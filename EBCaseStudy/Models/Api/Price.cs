using System;
using System.Text.Json.Serialization;

namespace EBCaseStudy.Models.Api
{
    public class Price
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("product")]
        public Product Product { get; set; }
    }
}
