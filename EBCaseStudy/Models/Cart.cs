using System.Collections.Generic;
using System.Linq;

namespace EBCaseStudy.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(i => i.Quantity * i.Price);
    }
}
