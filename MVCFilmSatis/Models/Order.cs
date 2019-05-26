using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPaid { get; set; }
        public decimal SubTotal { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public virtual Order Order { get; set; }
        public virtual Movie Movie { get; set; }

        public OrderItem()
        {
            Count = 1;
        }
    }
}