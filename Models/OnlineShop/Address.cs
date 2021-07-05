using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.OnlineShop
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Address_ID { get; set; }
        [Display(Name = "Street Number")]
        public int street_number { get; set; }
        [Display(Name = "Street Name")]
        public string street_name { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "Zip-Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}