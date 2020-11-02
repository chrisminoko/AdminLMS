using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.OnlineShop
{
    public class Payment
    {
        [Key]
        [Display(Name = "ID")]
        public int payment_ID { get; set; }
        [Display(Name = "Customer")]
        [ForeignKey("Customer")]
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public double AmountPaid { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string PaymentFor { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [ForeignKey("Order")]
        public string Order_ID { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
    }
}