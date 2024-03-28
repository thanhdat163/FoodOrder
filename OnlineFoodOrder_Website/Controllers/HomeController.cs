using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;

namespace OnlineFoodOrder_Website.Controllers
{
    public class HomeController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["OrderInvoice"] != null)
            {
                Session["OrderInvoice"] = null;
            }
            ViewBag.Category = db.Categories.Where(x => x.IsActive == true);
            var lstProduct = db.Products.Where(x => x.IsActive == true);
            return View(lstProduct);
        }

        public ActionResult LoadMenu()
        {
            if(Session["OrderInvoice"] != null)
            {
                Session["OrderInvoice"] = null;
            }
            ViewBag.Category = db.Categories.Where(x => x.IsActive == true);
            var lstProduct = db.Products.Where(x => x.IsActive == true);
            return View(lstProduct);
        }


        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user, FormCollection form)
        {
            User checkUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
            if (checkUser != null)
            {
                ViewBag.Message = "UserName already exists, please choose another UserName.";
                return View(user);
            }
            else
            {
                if (user.Passwords != form["txtConfirmPassword"])
                {
                    ViewBag.Message = "Password and Confirm Password do not match.";
                    return View(user);
                }
                else
                {
                    user.IsAdmin = false;
                    DateTime createDate = DateTime.Now;
                    user.CreateDate = createDate;
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Message = "Register success!";
                    return View();
                }
            }
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var checkUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName && x.Passwords == user.Passwords);
            if (checkUser == null)
            {
                ViewBag.Message = "UserName or Password is incorrect";
                return View();
            }
            else
            {
                Session["User"] = checkUser;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index");
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