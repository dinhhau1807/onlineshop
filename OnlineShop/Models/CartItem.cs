using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class CartItem
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}