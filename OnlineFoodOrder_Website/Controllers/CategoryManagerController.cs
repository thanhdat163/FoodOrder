using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OnlineFoodOrder_Website.Models;
using System.IO;
using PagedList;

namespace OnlineFoodOrder_Website.Controllers
{
    public class CategoryManagerController : Controller
    {
        // GET: CategoryManager

        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();

        public ActionResult SearchCategory(int? page, string SearchString)
        {

            int pageNum = page ?? 1;
            int pageSize = 2;
            var lstCategory = db.Categories;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var lstCategorySeach = db.Categories.Where(x => x.CategoryName.Contains(SearchString));
                return View(lstCategorySeach.OrderBy(x => x.CategoryID).ToPagedList(pageNum, pageSize));
            }
            return View(lstCategory.OrderBy(x => x.CategoryID).ToPagedList(pageNum, pageSize));

        }

        [HttpGet]
        public ActionResult AddCategory(int? page, string SearchString)
        {
            int pageNum = page ?? 1;
            int pageSize = 2;
            var lstCategory = db.Categories;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var lstCategorySeach = db.Categories.Where(x => x.CategoryName.Contains(SearchString));
                return View(lstCategorySeach.OrderBy(x => x.CategoryID).ToPagedList(pageNum, pageSize));
            }
            return View(lstCategory.OrderBy(x => x.CategoryID).ToPagedList(pageNum, pageSize));
        }

        [HttpPost]
        public ActionResult AddCategory(FormCollection form)
        {
            if (form == null)
            {
                return View();
            }
            else
            {
                var isActive = form["txtIsActive"];
                string date = form["txtCreateDate"];
                Category category = new Category();
                category.CategoryName = form["txtCategoryName"];
                category.CreateDate = DateTime.Parse(date);
                category.IsActive = Boolean.Parse(isActive);
                db.Categories.Add(category);
                db.SaveChanges();
                var lstCategory = db.Categories;
                return RedirectToAction("AddCategory", new { page = 1 });
            }
        }

        [HttpGet]
        public ActionResult UploadImage(int? categoryID)
        {
            var category = db.Categories.SingleOrDefault(x => x.CategoryID == categoryID);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult UploadImage(int categoryID, HttpPostedFileBase file)
        {
            int categoryId = (int)categoryID;
            try
            {
                if (file.ContentLength > 0)
                {
                    string _filename = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Image"), _filename);
                    file.SaveAs(_path);
                    var category = db.Categories.SingleOrDefault(x => x.CategoryID == categoryId);
                    if (category != null)
                    {
                        string fileImage = (string)file.FileName;
                        category.ImageUrl = fileImage;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("UploadImage", new { CategoryID = categoryId });
            }
            catch
            {
                return RedirectToAction("AddCategory");
            }
        }

        [HttpGet]
        public ActionResult UpdateCategory(int? categoryID)
        {
            var category = db.Categories.SingleOrDefault(x => x.CategoryID == categoryID);
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category updatecategory)
        {
            var category = db.Categories.SingleOrDefault(x => x.CategoryID == updatecategory.CategoryID);
            category.CategoryName = updatecategory.CategoryName;
            category.IsActive = updatecategory.IsActive;
            db.SaveChanges();
            return RedirectToAction("AddCategory");
        }

        public ActionResult DeleteCategory(int? categoryID)
        {
            var category = db.Categories.SingleOrDefault(x => x.CategoryID == categoryID);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("AddCategory");
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