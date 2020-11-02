
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Affiliate_Service
    {
        private ApplicationDbContext ModelsContext;
        public Affiliate_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }

        public List<Affiliate> GetAffiliates()
        {
            return ModelsContext.Affiliates.ToList();
        }
        public List<Affiliate_Joiner> GetAffiliateJoiners()
        {
            return ModelsContext.Affiliate_Joiners.ToList();
        }

        public bool IsAJoiner(string joiner_email)
        {
            return ModelsContext.Affiliate_Joiners.Count(x => x.New_Customer_Email == joiner_email) > 0;
        }
        public Affiliate GetJoinerAffiliate(string joiner_email)
        {
            return ModelsContext.Affiliate_Joiners.FirstOrDefault(x => x.New_Customer_Email == joiner_email) != null ? ModelsContext.Affiliate_Joiners.FirstOrDefault(x => x.New_Customer_Email == joiner_email).Affiliate : null;
        }
        public void PayAffiliates(string buyer_email, decimal amount_paid)
        {
            decimal percentage = (decimal)0.0025;
            Affiliate affiliate;
            int count = 1;
            while (count <= 4)
            {
                if (IsAJoiner(buyer_email))
                {
                    affiliate = GetJoinerAffiliate(buyer_email);
                    try
                    {
                        var balance = GetAccountBalance(affiliate.Affiliate_Key.ToString());
                        var benefit = balance + CalculateBenefit(amount_paid, percentage);
                        AddDeposit( new Models.OnlineShop.DepositTrans()
                        {
                            Affiliate_Key = affiliate.Affiliate_Key.ToString(),
                            Joiner_Email = buyer_email,
                            Description = "Joiner purchase earnings",
                            Amount = CalculateBenefit(amount_paid, percentage),
                            Remaining_Balance = benefit,
                            Transaction_Date = DateTime.Now
                        });
                    }
                    catch (Exception ex) { }
                    buyer_email = affiliate.Email;
                    percentage /= 2;
                }
                count++;
            }
        }
        public List<Transaction> GetTransactions()
        {
            return ModelsContext.Transactions.ToList();
        }
        public decimal CalculateBenefit(decimal amount, decimal percent)
        {
            return amount * percent;
        }
        public void AddDeposit(Models.OnlineShop.DepositTrans deposit)
        {
            try
            {
                ModelsContext.DepositsS.Add(deposit);
                ModelsContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public bool CanWithdraw(Withdraw withdraw)
        {
            return withdraw.Amount >= ModelsContext.WithdrawLevels.FirstOrDefault().Amount;
        }
        public void AddWithdrawal(Withdraw withdraw)
        {
            try
            {
                if (CanWithdraw(withdraw))
                {
                    ModelsContext.Withdrawals.Add(withdraw);
                    ModelsContext.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }
        public decimal GetAccountBalance(string affiliate_key)
        {
            decimal amount = 0;
            foreach (var item in ModelsContext.Transactions.ToList().Where(x => x.Affiliate_Key == affiliate_key))
            {
                amount += item.Amount;
            }
            return amount;
        }
    }
}