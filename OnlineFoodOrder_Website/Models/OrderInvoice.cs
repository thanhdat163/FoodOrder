using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodOrder_Website.Models
{
    public class OrderInvoice
    {
        public string OrderNo { get; set; }
        public int UserID { get; set; }
        public string ProductName { get; set; } 
        public int ProductID { get; set; }  
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalItemPrice { get; set; }

        public OrderInvoice(string orderNo, int userID, string productName, int productID, int? quantity, decimal? price, decimal? totalItemPrice)
        {
            OrderNo = orderNo;
            UserID = userID;
            ProductName = productName;
            ProductID = productID;
            Quantity = quantity;
            Price = price;
            TotalItemPrice = totalItemPrice;
        }

        public OrderInvoice()
        {

        }
    }
}