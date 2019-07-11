using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        OnlineShopDbContext _context = null;

        public OrderDetailDao()
        {
            _context = new OnlineShopDbContext();
        }

        public bool Insert(OrderDetail detail)
        {
            try
            {
                _context.OrderDetails.Add(detail);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
