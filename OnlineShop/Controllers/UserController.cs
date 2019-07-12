using Model.Dao;
using Model.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        Name = model.Name,
                        Phone = model.Phone,
                        Email = model.Email,
                        Address = model.Address,
                        CreatedDate = DateTime.Now,
                        Status = true
                    };

                    var result = dao.Insert(user);

                    if (result>0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng kí không thành công.");
                    }
                }
            }

            return View(model);
        }
    }
}