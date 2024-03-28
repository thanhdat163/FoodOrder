using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;
using System.IO;
using PagedList;



namespace OnlineFoodOrder_Website.Controllers
{
    public class UserController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: User
        [HttpGet]
        public ActionResult ProfileUser(int? userID)
        {
            if (userID == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var user = db.Users.SingleOrDefault(x => x.UserID == userID);
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult ProfileUser(User user, FormCollection form)
        {
            int userID = (int)user.UserID;
            var updateUser = db.Users.SingleOrDefault(x => x.UserID == userID);
            updateUser.FullName = user.FullName;
            updateUser.Email = user.Email;
            updateUser.Mobile = user.Mobile;
            updateUser.UserAddress = user.UserAddress;
            updateUser.PostCode = user.PostCode;
            updateUser.UserName = user.UserName;
            updateUser.CreateDate = user.CreateDate;
            db.SaveChanges();
            if (user.Passwords == updateUser.Passwords)
            {
                if (form["txtRepassword"] == null || form["txtRepassword"] == "")
                {
                    ViewBag.Message = "Update success";
                    return View(user);
                }
                else
                {
                    ViewBag.Message = "Do not input comfirm password if you do not change password";
                    return View(user);
                }
            }
            else
            {
                if (user.Passwords == form["txtRepassword"])
                {
                    updateUser.Passwords = user.Passwords;
                    db.SaveChanges();
                    ViewBag.Message = "Update success";
                    return View(user);
                }
                else
                {
                    ViewBag.Message = "Password and Confirm Password are not match";
                    return View(user);
                }
            }
        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file, int UserID)
        {
            int userID = (int)UserID;
            try
            {
                if (file.ContentLength > 0)
                {
                    string _fileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Image"), _fileName);
                    file.SaveAs(_path);
                    var user = db.Users.SingleOrDefault(x => x.UserID == userID);
                    if (user != null)
                    {
                        string filename = (string)file.FileName;
                        user.ImageUrl = filename;
                        db.SaveChanges();
                    }
                }
                ViewBag.Message = "Upload Success";
                return RedirectToAction("ProfileUser", new { UserID = userID });
            }
            catch
            {
                ViewBag.Message = "Upload Fail";
                return RedirectToAction("ProfileUser", new { UserID = userID });
            }
        }
        public ActionResult DisplayPurchasedHistory(int? page, int? userID)
        {
            //Get list Pursechased History Begin
            List<PurchasedHistory> purchasedHistory = new List<PurchasedHistory>();
            // Get userID 
            User user = db.Users.SingleOrDefault(x => x.UserID == userID);
            //Get List Order of a User
            var orderedOfUser = db.Orders.Where(x => x.UserID == userID).ToList();
            // Get Distince List Payment by PaymentID
            var distinctPaymentID = orderedOfUser.Where(p => p.PaymentID != null).GroupBy(p => p.PaymentID).Select(gr => gr.FirstOrDefault()).ToList();
            // Add list Purchased History
            foreach (var distinct in distinctPaymentID)
            {
                PurchasedHistory purschased = new PurchasedHistory();
                purschased.UserID = user.UserID;
                purschased.PaymentID = distinct.PaymentID;
                Payment payment = db.Payments.SingleOrDefault(x => x.PaymentID == purschased.PaymentID);
                purschased.PaymentMode = payment.PaymentMode;
                purschased.CardNo = payment.CardNo;
                var orderOfEachPurchased = orderedOfUser.Where(x => x.PaymentID == purschased.PaymentID).ToList();
                // Add list item after each of ordering
                List<PurchasedItemOfEachCart> purchasedItemOfEachCarts = new List<PurchasedItemOfEachCart>();
                int count = orderOfEachPurchased.Count;
                if (count == 1)
                {
                    var onlyOneOrder = orderedOfUser.SingleOrDefault(x => x.PaymentID == purschased.PaymentID);
                    PurchasedItemOfEachCart purchasedItem = new PurchasedItemOfEachCart();
                    purchasedItem.ProductId = onlyOneOrder.ProductID;
                    Product product = db.Products.SingleOrDefault(x => x.ProductID == purchasedItem.ProductId);
                    purchasedItem.ProductName = product.ProductName;
                    purchasedItem.Price = product.Price;
                    purchasedItem.Quantity = onlyOneOrder.Quantity;
                    purchasedItem.ItemPriceTotal = purchasedItem.Quantity * purchasedItem.Price;
                    purchasedItem.OrderCode = onlyOneOrder.OrderNo;
                    purchasedItem.StatusItem = onlyOneOrder.Statuss;
                    purchasedItemOfEachCarts.Add(purchasedItem);

                }
                else
                {
                    foreach (var purchase in orderOfEachPurchased)
                    //for (int y = 0; y < orderOfEachPurchased.Count; y++)
                    {
                        PurchasedItemOfEachCart purchasedItem = new PurchasedItemOfEachCart();
                        purchasedItem.ProductId = purchase.ProductID;
                        Product product = db.Products.SingleOrDefault(x => x.ProductID == purchasedItem.ProductId);
                        purchasedItem.ProductName = product.ProductName;
                        purchasedItem.Price = product.Price;
                        purchasedItem.Quantity = purchase.Quantity;
                        purchasedItem.ItemPriceTotal = purchasedItem.Quantity * purchasedItem.Price;
                        purchasedItem.OrderCode = purchase.OrderNo;
                        purchasedItem.StatusItem = purchase.Statuss;
                        purchasedItemOfEachCarts.Add(purchasedItem);
                    }
                }
                purschased.purchasedItemOfEachCarts = purchasedItemOfEachCarts;
                purschased.TotalCart = purchasedItemOfEachCarts.Sum(x => x.ItemPriceTotal);

                purchasedHistory.Add(purschased);
            }
            //End
            int pageNum = page ?? 1;
            int pageSize = 4;
            return View(purchasedHistory.OrderBy(x => x.PaymentID).ToPagedList(pageNum, pageSize));
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