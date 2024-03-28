using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;
using PagedList;

namespace OnlineFoodOrder_Website.Controllers
{
    public class OrderManagerController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1 ();
        // GET: OrderManager
        public ActionResult OrderList(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 15;
            var lstOrder = db.Orders;
            return View(lstOrder.OrderBy(x => x.OrderDetailsID).ToPagedList(pageNum, pageSize));
        }

        public ActionResult SearchOrder(int? page, string SearchString)
        {
            int pageNum = page ?? 1;
            int pageSize = 15;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var searchOrder = db.Orders.Where(x => x.OrderNo.Contains(SearchString));
                return View(searchOrder.OrderBy(x => x.OrderDetailsID).ToPagedList(pageNum, pageSize));
            }
            else
            {
                return RedirectToAction("OrderList");
            }
        }

        [HttpGet]
        public ActionResult EditStatusOrder(int orderDetailID, FormCollection form)
        {
            Order orderDetail = db.Orders.SingleOrDefault(x => x.OrderDetailsID == orderDetailID);
            form["txtStatus"] = orderDetail.Statuss;
            ViewBag.OrderCode = orderDetail.OrderNo;
            ViewBag.LstOrderByOrderNo = db.Orders.Where(x => x.PaymentID == orderDetail.PaymentID).ToList();
            return View(orderDetail);
        }

        [HttpPost]
        public ActionResult EditStatusOrder(Order order, FormCollection form)
        {
            Order updateOrder = db.Orders.SingleOrDefault(x => x.OrderDetailsID == order.OrderDetailsID);
            updateOrder.Statuss = form["txtStatus"];
            form["txtStatus"] = updateOrder.Statuss;
            ViewBag.OrderCode = order.OrderNo;
            ViewBag.LstOrderByOrderNo = db.Orders.Where(x => x.PaymentID == updateOrder.PaymentID).ToList();
            db.SaveChanges();
            return View(order);
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