
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Address_Service
    {
        private ApplicationDbContext ModelsContext;

        public Address_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }
        public List<Shipping_Address> GetOrderAddresses()
        {
            return ModelsContext.Shipping_Addresses.ToList();
        }
        public void AddShippingAddress(Shipping_Address order_Address)
        {
            try
            {
                ModelsContext.Shipping_Addresses.Add(order_Address);
                ModelsContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public Shipping_Address GetShippingAddress(int address_id)
        {
            return ModelsContext.Shipping_Addresses.FirstOrDefault(x => x.Address_ID == address_id);
        }
        public Shipping_Address GetShippingAddress(string order_id)
        {
            return ModelsContext.Shipping_Addresses.FirstOrDefault(x => x.Order_ID == order_id);
        }
    }
}