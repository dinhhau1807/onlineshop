using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        OnlineShopDbContext _context = null;

        public MenuDao()
        {
            _context = new OnlineShopDbContext();
        }

        public List<Menu> ListByGroupId(int groupId)
        {
            return _context.Menus.Where(x => x.TypeID == groupId).ToList();
        }
    }
}
