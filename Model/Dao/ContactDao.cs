using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        public OnlineShopDbContext _context = null;

        public ContactDao()
        {
            _context = new OnlineShopDbContext();
        }

        public Contact GetActiveContact()
        {
            return _context.Contacts.Single(x => x.Status == true);
        }

        public int InsertFeedback(Feedback fb)
        {
            _context.Feedbacks.Add(fb);
            _context.SaveChanges();
            return fb.ID;
        }
    }
}
