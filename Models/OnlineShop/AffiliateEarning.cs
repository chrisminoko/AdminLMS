using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackEnd.Models.OnlineShop
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ref { get; set; }
        public string Affiliate_Key { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Remaining_Balance { get; set; }
        public DateTime Transaction_Date { get; set; }
    }

    public class DepositTrans : Transaction
    {
        public string Joiner_Email { get; set; }
    }

    public class Withdraw : Transaction
    {

    }
    public class Transfer : Transaction
    {
        public string To_Affiliate { get; set; }
    }

    public class WithdrawLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Level_ID { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}