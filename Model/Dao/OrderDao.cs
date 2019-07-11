using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext _context = null;

        public OrderDao()
        {
            _context = new OnlineShopDbContext();
        }

        public long Insert(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.ID;
        }
    }
}
