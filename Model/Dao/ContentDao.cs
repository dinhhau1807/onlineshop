using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContentDao
    {
        OnlineShopDbContext _context = null;

        public ContentDao()
        {
            _context = new OnlineShopDbContext();
        }

        public Content GetByID(long id)
        {
            return _context.Contents.Find(id);
        }
    }
}
