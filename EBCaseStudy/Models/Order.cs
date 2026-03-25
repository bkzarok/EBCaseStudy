using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EBCaseStudy.Models
{
    public enum OrderStatus
    {
        New,
        Approved,
        Rejected
    }

    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
