using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models.OnlineShop
{
    public class DeliveryModel
    {
        [Key]
        public int DeliveryId { get; set; }
        public string OrderID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DeliveryDate { get; set; }
    }
}