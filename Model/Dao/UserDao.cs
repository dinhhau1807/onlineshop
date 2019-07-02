using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserDao
    {
        OnlineShopDbContext _context = null;

        public UserDao()
        {
            _context = new OnlineShopDbContext();
        }

        public long Insert(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity.ID;
        }

        public bool Update(User entity)
        {
            var user = _context.Users.Find(entity.ID);
            user.Name = entity.Name;
            user.Address = entity.Address;
            user.Email = entity.Email;
            user.ModifedBy = entity.ModifedBy;
            user.ModifiedDate = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<User> ListAllPaging(int page, int pageSize)
        {
            return _context.Users.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        public User GetById(string userName)
        {
            return _context.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public int Login(string userName, string passWord)
        {
            var result = _context.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }
    }
}
