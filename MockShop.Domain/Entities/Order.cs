using System;
using System.Collections.Generic;
using System.Text;

namespace MockShop.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Foreign Key
                                        
        public DateTimeOffset OrderDate { get; set; } // PostgreSQL TIMESTAMPTZ

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";
        public ICollection<OrderItem> Items { get; set; }
    }
}
