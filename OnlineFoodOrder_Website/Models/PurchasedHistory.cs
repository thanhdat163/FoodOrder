using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodOrder_Website.Models
{
    public class PurchasedHistory
    {
        public int UserID { get; set; }
        public Nullable<int> PaymentID { get; set; }
        public string CardNo { get; set; }
        public string PaymentMode { get; set; }
        public List<PurchasedItemOfEachCart> purchasedItemOfEachCarts { get; set; } 
        public Nullable<decimal> TotalCart { get; set; }

        public PurchasedHistory()
        {

        }

        public PurchasedHistory(int userID, int? paymentID, string cardNo, string paymentMode, List<PurchasedItemOfEachCart> purchasedItemOfEachCarts, decimal? totalCart)
        {
            UserID = userID;
            PaymentID = paymentID;
            CardNo = cardNo;
            PaymentMode = paymentMode;
            this.purchasedItemOfEachCarts = purchasedItemOfEachCarts;
            TotalCart = totalCart;  
        }
    }
}