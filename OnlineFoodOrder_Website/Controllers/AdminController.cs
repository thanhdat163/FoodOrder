using OnlineFoodOrder_Website.Models;
using System.Linq;
using System.Web.Mvc;

namespace OnlineFoodOrder_Website.Controllers
{
    public class AdminController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: Admin
        public ActionResult AdminHome()
        {
            ViewBag.Categories = CountCategory();
            ViewBag.Products = CountProduct();
            ViewBag.TotalOrder = CountOrder();
            ViewBag.Delevered = CountDelivery();
            ViewBag.Pending = CountPending();
            ViewBag.User = CountUser();
            ViewBag.Total = TotalSolidAmont();
            ViewBag.FeedBack = CountFeedBack();
            return View();
        }

        public int CountCategory()
        {
            var lstCategory = db.Categories.Where(x => x.IsActive == true).ToList();
            int count = lstCategory.Count;
            return count;
        }

        public int CountProduct()
        {
            var lstProduct = db.Products.Where(x => x.IsActive == true).ToList();
            int count = lstProduct.Count;
            return count;
        }

        public int CountOrder()
        {
            var lstOrder = db.Orders.ToList();
            int count = lstOrder.Count;
            return count;
        }

        public int CountDelivery()
        {
            var lstDelivery = db.Orders.Where(x => x.Statuss.Contains("Delivered")).ToList();
            int count = lstDelivery.Count;
            return count;
        }

        public int CountPending()
        {
            var lstPending = db.Orders.Where(x => x.Statuss.Contains("Pending")).ToList();
            int count = lstPending.Count;
            return count;
        }
        public int CountUser()
        {
            var lstUser = db.Users.ToList();
            int count = lstUser.Count;
            return count;
        }

        public int TotalSolidAmont()
        {
            var lstOrder = db.Orders.ToList();
            int totalRevenue = 0;
            foreach(var order in lstOrder)
            {
                Product product = db.Products.SingleOrDefault(x => x.ProductID == order.ProductID);
                int totalPrice = (int)(product.Price * order.Quantity);
                totalRevenue += totalPrice;
            }

            return totalRevenue;
        }

        public int CountFeedBack()
        {
            var lstFeedBack = db.Contacts.ToList();
            int count = lstFeedBack.Count;
            return count;
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