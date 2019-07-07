using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SlideDao
    {
        OnlineShopDbContext _context = null;

        public SlideDao()
        {
            _context = new OnlineShopDbContext();
        }

        public List<Slide> ListAll()
        {
            return _context.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
    }
}
