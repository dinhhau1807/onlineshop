﻿using Common;
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

        public long InsertForFacebook(User entity)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                _context.Users.Add(entity);
                _context.SaveChanges();
                return entity.ID;
            }

            return user.ID;
        }

        public bool Update(User entity)
        {
            var user = _context.Users.Find(entity.ID);
            user.Name = entity.Name;
            if (!string.IsNullOrEmpty(entity.Password))
            {
                user.Password = entity.Password;
            }
            user.Address = entity.Address;
            user.Email = entity.Email;
            user.ModifedBy = entity.ModifedBy;
            user.ModifiedDate = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = _context.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public User GetById(string userName)
        {
            return _context.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public User ViewDetail(int id)
        {
            return _context.Users.Find(id);
        }

        public List<string> GetListCredential(string userName)
        {
            var user = _context.Users.Single(x => x.UserName == userName);

            var data = (from a in _context.Credentials
                        join b in _context.UserGroups on a.UserGroupID equals b.ID
                        join c in _context.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });

            return data.Select(x => x.RoleID).ToList();
        }

        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = _context.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
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
                    else
                    {
                        return -3;
                    }
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

        public bool Delete(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var user = _context.Users.Find(id);
            user.Status = !user.Status;

            _context.SaveChanges();

            return user.Status;
        }

        public bool CheckUserName(string userName)
        {
            return _context.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return _context.Users.Count(x => x.Email == email) > 0;
        }
    }
}
