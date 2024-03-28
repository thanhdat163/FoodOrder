using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodOrder_Website.Models
{
    public class PurchasedItemOfEachCart
    {
        public string ProductName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }

        public Nullable<decimal> ItemPriceTotal { get; set; }
        
        public string OrderCode { get; set; }
        public string StatusItem { get; set; }

        public PurchasedItemOfEachCart()
        {

        }

        public PurchasedItemOfEachCart(string productName, int? productID, decimal? price, int? quantity, decimal? itemPriceTotal, string orderCode, string statusitem)
        {
            ProductName = productName;
            ProductId = productID;
            Price = price;
            Quantity = quantity;
            ItemPriceTotal = itemPriceTotal;
            OrderCode = orderCode;
            StatusItem = statusitem;
        }
    }
}