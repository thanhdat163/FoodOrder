using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineFoodOrder_Website.Models;

namespace OnlineFoodOrder_Website.Models
{
    public class CartItem
    {
        public CartItem()
        {

        }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }

        public int UserID { get; set; }
        public Nullable<decimal> ItemPriceTotal { get; set; }

        public CartItem(int productID, int userID)
        {
            using (OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1())
            {
                this.ProductID = productID;
                Product product = db.Products.SingleOrDefault(x => x.ProductID == productID);
                this.ProductName = product.ProductName;
                this.ProductImg = product.ImageUrl;
                this.Price = product.Price.Value;
                this.Quantity = 1;
                this.ItemPriceTotal = Price * Quantity;
                this.UserID = userID;
            }
        }
    }
}