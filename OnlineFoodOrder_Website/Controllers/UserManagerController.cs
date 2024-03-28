using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OnlineFoodOrder_Website.Models;
using System.Data.Entity.Core.Common.CommandTrees;

namespace OnlineFoodOrder_Website.Controllers
{
    public class UserManagerController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: AdminManager
        public ActionResult ListUser(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 5;
            var lstUser = db.Users;
            return View(lstUser.OrderBy(x => x.UserID).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult UpdateUserRole(int? UserID)
        {
            var updateUser = db.Users.SingleOrDefault(x => x.UserID == UserID);
            if (updateUser == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                return View(updateUser);
            }
        }

        [HttpPost]
        public ActionResult UpdateUserRole(User user, FormCollection form)
        {
            if(user == null)
            {
                ViewBag.Message = "Update fail.";
                return View(user);
            } else
            {
                var updateUser = db.Users.SingleOrDefault(x => x.UserID == user.UserID);
                updateUser.IsAdmin = user.IsAdmin;
                updateUser.ImageUrl = form["txtImageUrl"];
                db.SaveChanges();
                ViewBag.Message = "Update success";
                return View(updateUser);
            }
        }

        public ActionResult DeleteUser(int UserID)
        {
            var deleteUser = db.Users.SingleOrDefault(x => x.UserID == UserID);
            if (deleteUser == null)
            {
                Response.StatusCode = 404;
                return null;
            } else
            {
                db.Users.Remove(deleteUser);
                var listOrder = db.Orders.Where(x => x.UserID == UserID).ToList();
                foreach(var item in listOrder)
                {
                    db.Orders.Remove(item);
                }
                db.SaveChanges();
                return RedirectToAction("ListUser");
            }
               
        }

        public ActionResult SearchUser(int? page, string SearchString)
        {
            int pageNum = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var searchUser = db.Users.Where(x => x.UserName.Contains(SearchString));
                return View(searchUser.OrderBy(x => x.UserID).ToPagedList(pageNum, pageSize));
            } else
            {
                return RedirectToAction("ListUser");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}