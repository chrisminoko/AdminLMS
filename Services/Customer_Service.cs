
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Customer_Service
    {
        private ApplicationDbContext ModelsContext;
        public Customer_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }
        public List<Customer> GetCustomers()
        {
            return ModelsContext.Customers.ToList();
        }
        public bool AddCustomer(Customer customer, string affiliate_key)
        {
            try
            {
                ModelsContext.Customers.Add(customer);
                ModelsContext.Affiliates.Add(new Affiliate() { Email = customer.Email });
                ModelsContext.SaveChanges();
                if (!String.IsNullOrEmpty(affiliate_key))
                {
                    var affiliate = ModelsContext.Affiliates.Find(Guid.Parse(affiliate_key));
                    if (affiliate != null)
                    {
                        ModelsContext.Affiliate_Joiners.Add(new Affiliate_Joiner()
                        {
                            Affiliate_Key = affiliate.Affiliate_Key,
                            New_Customer_Email = customer.Email,
                            Email = affiliate.Email,
                            used = false
                        });
                        ModelsContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                ModelsContext.Entry(customer).State = EntityState.Modified;
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Customer GetCustomer(string email)
        {
            return ModelsContext.Customers.FirstOrDefault(x => x.Email == email);
        }

        public string GetGender(string id_num)
        {
            if (Convert.ToInt16(id_num.Substring(7, 1)) >= 5)
                return "Male";
            else
                return "Female";
        }
    }
}