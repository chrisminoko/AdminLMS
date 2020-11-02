
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Category_Service
    {
        private ApplicationDbContext ModelsContext;
        public Category_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }
        public List<Category> GetCategories()
        {
            return ModelsContext.Categories.ToList();
        }
        public bool AddCategory(Category category)
        {
            try
            {
                ModelsContext.Categories.Add(category);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool UpdateCategory(Category category)
        {
            try
            {
                ModelsContext.Entry(category).State = EntityState.Modified;
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool RemoveCategory(Category category)
        {
            try
            {
                ModelsContext.Categories.Remove(category);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Category GetCategory(int? category_id)
        {
            return ModelsContext.Categories.Find(category_id);
        }
    }
}