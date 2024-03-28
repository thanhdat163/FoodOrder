using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;
using PagedList;
using System.IO;

namespace OnlineFoodOrder_Website.Controllers
{
    public class ProductManagerController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: ProductManager
        [HttpGet]
        public ActionResult AddProduct(int? page, string SearchString)
        {
            ViewBag.CategoryID = db.Categories.Where(x => x.IsActive == true);
            int pageNum = page ?? 1;
            int pageSize = 5;
            var product = db.Products;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var lstSearch = db.Products.Where(x => x.ProductName.Contains(SearchString));
                return View(lstSearch.OrderBy(x => x.ProductID).ToPagedList(pageNum, pageSize));
            }
            return View(product.OrderBy(x => x.ProductID).ToPagedList(pageNum, pageSize));
        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {
            Product product = new Product();
            product.ProductName = form["txtProductName"];
            product.Descriptions = form["txtDescription"];
            product.Price = Convert.ToDecimal(form["txtPrice"]);
            product.Quantity = Convert.ToInt32(form["txtQuantity"]);
            product.CategoryID = Convert.ToInt32(form["txtCategoryID"]);
            product.CreateDate = Convert.ToDateTime(form["txtCreateDate"]);
            product.IsActive = Convert.ToBoolean(form["txtIsActive"]);
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("AddProduct");
        }

        [HttpGet]
        public ActionResult UpdateProduct(int? ProductID)
        {
            var product = db.Products.FirstOrDefault(x => x.ProductID == ProductID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            if(product == null)
            {
                Response.StatusCode = 404;
                return null;
            } else
            {
                return View(product);
            }
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product updateProduct)
        {
            Product product = db.Products.SingleOrDefault(x => x.ProductID == updateProduct.ProductID);
            if(product == null)
            {
                Response.StatusCode = 404;
                return null;
            } else
            {
                product.ProductName = updateProduct.ProductName;
                product.Price = updateProduct.Price;
                product.Quantity = updateProduct.Quantity;
                product.Descriptions = updateProduct.Descriptions;
                product.IsActive = updateProduct.IsActive;
                db.SaveChanges();
                return RedirectToAction("AddProduct");
            }
        }

        [HttpGet]
        public ActionResult UploadImage(int? ProductID)
        {
            var product = db.Products.SingleOrDefault(x => x.ProductID == ProductID);
            return View(product);
        }

        public ActionResult UploadImage(int? ProductID, HttpPostedFileBase file)
        {
            int productID = (int)ProductID;
            try
            {
                if(file.ContentLength > 0)
                {
                    string _filename = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("../Image"), _filename);
                    file.SaveAs(_path);
                    var product = db.Products.SingleOrDefault(x => x.ProductID == ProductID);
                    if(product != null)
                    {
                        string filename = (string)file.FileName;
                        product.ImageUrl = filename;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("UploadImage", new { ProductID = productID});
            }
            catch
            {
                return RedirectToAction("AddProduct");
            }
        }
        
        public ActionResult DeleteProduct(int? ProductID)
        {
            int productID = (int)ProductID;
            var product = db.Products.SingleOrDefault(x => x.ProductID == productID);
            if(product == null)
            {
                Response.StatusCode = 404;
                return null;
            } else
            {
                var lstOrder = db.Orders.Where(x => x.ProductID == ProductID).ToList();
                foreach(var item in lstOrder)
                {
                    Order order = db.Orders.SingleOrDefault(x => x.OrderDetailsID == item.OrderDetailsID);
                    db.Orders.Remove(order);
                    db.SaveChanges();
                }
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("AddProduct");
            }
        }

        public ActionResult SearchProduct(int? page, string SearchString)
        {
            ViewBag.CategoryID = db.Categories;
            int pageNum = page ?? 1;
            int pageSize = 5;
            var product = db.Products;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var lstSearch = db.Products.Where(x => x.ProductName.Contains(SearchString));
                return View(lstSearch.OrderBy(x => x.ProductID).ToPagedList(pageNum, pageSize));
            }
            return View(product.OrderBy(x => x.ProductID).ToPagedList(pageNum, pageSize));
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