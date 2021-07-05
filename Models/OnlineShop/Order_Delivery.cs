using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models.OnlineShop
{
    public class Order_Delivery : Delivery
    {
        public string Contact_Person { get; set; }
        public string Telephone { get; set; }
        public bool Recieved_In_GoodOrder { get; set; }
        public string recipient { get; set; }
        // public string signature { get; set; }
        public DateTime date { get; set; }
    }
}