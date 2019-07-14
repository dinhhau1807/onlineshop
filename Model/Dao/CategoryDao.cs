using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext _context = null;

        public CategoryDao()
        {
            _context = new OnlineShopDbContext();
        }

        public List<Category> ListAll()
        {
            return _context.Categories.Where(x => x.Status == true).ToList();
        }

        public long Insert(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.ID;
        }
    }
}
