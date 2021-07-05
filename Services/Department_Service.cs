
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Department_Service
    {
        private ApplicationDbContext ModelsContext;
        public Department_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }
        public List<Department> GetDepartments()
        {
            return ModelsContext.Departments.ToList();
        }
        public bool AddDepartment(Department department)
        {
            try
            {
                ModelsContext.Departments.Add(department);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool UpdateDepartment(Department department)
        {
            try
            {
                ModelsContext.Entry(department).State = EntityState.Modified;
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool RemoveDepartment(Department department)
        {
            try
            {
                ModelsContext.Departments.Remove(department);
                ModelsContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Department GetDepartment(int? department_id)
        {
            return ModelsContext.Departments.Find(department_id);
        }
    }
}