using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models.OnlineShop
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Range(0,5, ErrorMessage ="Rate must be between 0 and 5")]
        [Display(Name = "Rate 1 and 5")]
        public int Rating { get; set; }
        [Display(Name = "Review Text")]
        public string reviewText { get; set; }
        public int ItemCode { get; set; }
    }
}