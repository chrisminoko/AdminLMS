using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackEnd.Models.OnlineShop
{
    public class Shipping_Address : Address
    {
        public Shipping_Address()
        { }
        public string Building_Name { get; set; }
        public string Floor { get; set; }
        public string Contact_Number { get; set; }
        public string Address_Type { get; set; }// business, home etc.
        public string Comments { get; set; }

        [ForeignKey("Order")]
        public string Order_ID { get; set; }
        public virtual Order Order { get; set; }
    }
}