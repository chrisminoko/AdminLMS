using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEnd.Models.OnlineShop
{
    public class Affiliate
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Affiliate_Key { get; set; }
        [ForeignKey("Customer")]
        public string Email { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Affiliate_Joiner> Affiliate_Joiners { get; set; }
    }
}