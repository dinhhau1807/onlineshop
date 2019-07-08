using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext _context = null;

        public ProductCategoryDao()
        {
            _context = new OnlineShopDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return _context.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return _context.ProductCategories.Find(id);
        }
    }
}
