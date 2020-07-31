using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }
        public int PackageID { get; set; }
        public virtual Package Package { get; set; }
        public string UserEmail { get; set; }
        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiryDateDate { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set;}
        public decimal Amount { get; set; }
    }
}