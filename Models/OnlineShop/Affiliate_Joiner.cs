using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.OnlineShop
{
    public class Affiliate_Joiner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("Affiliate")]
        public Guid Affiliate_Key { get; set; }
        public string Email { get; set; }
        //[ForeignKey("Customer")]
        public string New_Customer_Email { get; set; }

        public bool used { get; set; }
        public virtual Affiliate Affiliate { get; set; }
    }
}