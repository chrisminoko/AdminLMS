using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Deposit
    {
        [Key]
        public int DepositID { get; set; }
        [Display(Name = "Name on Account")]
        public string NameOnAccount { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Deposit Amount")]
        [DataType(DataType.Currency)]
        public decimal DepositAmount { get; set; }
        [Display(Name = "Deposit Date")]
        [DataType(DataType.Date)]
        public DateTime DepositDate { get; set; }
        public string UserEmail { get; set; }
        public string ID { get; set; }
        [Display(Name = "Deposit Staus")]
        public string Status { get; set; }
        [Display(Name = "Deposit Code")]
        public string DepositTraceCode { get; set; }
        [Display(Name = "Deposit Proof")]
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }

    }
}