using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual List<Movie> Movies { get; set; } 

        public decimal SubTotal {
            get
            {
                return Movies.Sum(x => x.Price);
            }
        }

        public ShoppingCart()
        {
            CreateDate = DateTime.Now;
        }
    }
}