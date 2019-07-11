using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext _context = null;

        public ProductDao()
        {
            _context = new OnlineShopDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return _context.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        /// <summary>
        /// List feature product
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return _context.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRelatedProduct(long productId)
        {
            var product = _context.Products.Find(productId);
            return _context.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        /// <summary>
        /// Get list product by category 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = _context.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = from a in _context.Products
                        join b in _context.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.CategoryID == categoryID
                        select new ProductViewModel
                        {
                            CateMetaTitle = b.MetaTitle,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitle,
                            Price = a.Price
                        };

            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return model.ToList();
        }

        public Product ViewDetail(long id)
        {
            return _context.Products.Find(id);
        }
    }
}
