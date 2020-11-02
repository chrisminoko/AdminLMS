
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Item_Service
    {
        private ApplicationDbContext ModelsContext;
        public Item_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }
        public List<Item> GetItems()
        {
            return ModelsContext.Items.Include(i => i.Category).ToList();
        }
        public bool AddItem(Item item)
        {
            try
            {
                ModelsContext.Items.Add(item);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool UpdateItem(Item item)
        {
            try
            {
                ModelsContext.Entry(item).State = EntityState.Modified;
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool RemoveItem(Item item)
        {
            try
            {
                ModelsContext.Items.Remove(item);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Item GetItem(int? item_id)
        {
            return ModelsContext.Items.Find(item_id);
        }
    }
}