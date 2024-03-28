using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;

namespace OnlineFoodOrder_Website.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();   
        public ActionResult ViewCart(int UserID)
        {
            int userID = (int)UserID;
            decimal ToTalOfCart;
            List<CartItem> lstCart = ListItemCart();

            var CartOfUser = lstCart.Where(x => x.UserID == userID);


            if(CartOfUser == null)
            {
                ToTalOfCart = 0;
            }

            ToTalOfCart = (decimal)CartOfUser.Sum(x => x.ItemPriceTotal);

            ViewBag.TotalCart = ToTalOfCart;  
            return View(CartOfUser);
        }

        public List<CartItem> ListItemCart()
        {
            List<CartItem> cartItems = Session["ItemCart"] as List<CartItem>;
            if(cartItems == null)
            {
                cartItems = new List<CartItem>();
                Session["ItemCart"] = cartItems;    
            }
            return cartItems;
        }

        public ActionResult AddToCart(int ProductID, string strUrl)
        {
            User user = Session["User"] as User;
            int userID = (int)user.UserID;

            Product product = db.Products.SingleOrDefault(x => x.ProductID == ProductID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<CartItem> lstCart = ListItemCart();

            CartItem spCheck = lstCart.SingleOrDefault(x => x.ProductID == ProductID);
            if (spCheck != null)
            {
                if (spCheck.Quantity > product.Quantity)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                spCheck.Quantity++;
                spCheck.ItemPriceTotal = spCheck.Quantity * spCheck.Price;
                return Redirect(strUrl);
            }

            CartItem item = new CartItem(ProductID, userID);
            if (product.Quantity < item.Quantity)
            {
                Response.StatusCode = 404;
                return null;
            }
            lstCart.Add(item);

            return Redirect(strUrl);
        }

        public PartialViewResult CountItemCartPartial(int UserID)
        {
            List<CartItem> lstCart = ListItemCart();

            User user = db.Users.SingleOrDefault(x => x.UserID == UserID);

            var CartOfUser = lstCart.Where(x => x.UserID == UserID);   

            if (CartOfUser == null)
            {
                ViewBag.CountItem = 0;  
            } else
            {
                int count = (int)CartOfUser.Sum(x => x.Quantity);
                ViewBag.CountItem = count;
            } 
            return PartialView("CountItemCartPartial");
        }

        [HttpGet]
        public ActionResult UpdateCart(int ProductID, int UserID)
        {
            List<CartItem> lstCartUpdate = Session["ItemCart"] as List<CartItem>;
            var product = lstCartUpdate.SingleOrDefault(x => x.ProductID == ProductID && x.UserID == UserID);
            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateCart(CartItem cartItem)
        {
            List<CartItem> listCart = Session["ItemCart"] as List<CartItem>;
            CartItem updateCartItem = listCart.SingleOrDefault(x => x.ProductID == cartItem.ProductID && x.UserID == cartItem.UserID);
            updateCartItem.Quantity = cartItem.Quantity;
            updateCartItem.ItemPriceTotal = cartItem.Quantity * cartItem.Price;
            return View(updateCartItem);
        }

        public ActionResult DeleteCart(int ProductID, int UserID)
        {
            List<CartItem> lstCart = Session["ItemCart"] as List<CartItem>;
            CartItem cartdelete = lstCart.SingleOrDefault(x => x.ProductID == ProductID && x.UserID == UserID);
            lstCart.Remove(cartdelete);
            return RedirectToAction("ViewCart", new { UserID = UserID });
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