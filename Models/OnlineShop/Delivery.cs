using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.OnlineShop
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Delivery_ID { get; set; }
        [ForeignKey("Customer")]
        public string To { get; set; }
        [ForeignKey("Shipping_Address")]
        public int Address_ID { get; set; }
        [ForeignKey("Order")]
        public int Order_ID { get; set; }
        [ForeignKey("Invoice")]
        public int Invoice_ID { get; set; }

        public DateTime Scheduled_Date { get; set; }
        public bool Schedule_Confirmed { get; set; }
        public DateTime Date_DElivereed { get; set; }

        public Customer Customer { get; set; }
        public Shipping_Address Shipping_Address { get; set; }
        public Order Order { get; set; }
    }
}