using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class FooterDao
    {
        OnlineShopDbContext _context = null;

        public FooterDao()
        {
            _context = new OnlineShopDbContext();
        }

        public Footer GetFooter()
        {
            return _context.Footers.SingleOrDefault(x => x.Status == true);
        }
    }
}
