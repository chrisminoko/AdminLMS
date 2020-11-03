using BackEnd.Models.OnlineShop;
using BackEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackEnd.Controllers
{
    public class CategoriesController : Controller
    {
        Category_Service Category_Service = new Category_Service();
        Department_Service department_Service = new Department_Service();
        public CategoriesController()
        {

        }
        public ActionResult Index()
        {
            return View(Category_Service.GetCategories());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (Category_Service.GetCategory(id) != null)
                return View(Category_Service.GetCategory(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            ViewBag.Department_ID = new SelectList(department_Service.GetDepartments(), "Department_ID", "Department_Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            ViewBag.Department_ID = new SelectList(department_Service.GetDepartments(), "Department_ID", "Department_Name");
            if (ModelState.IsValid)
            {
                Category_Service.AddCategory(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Department_ID = new SelectList(department_Service.GetDepartments(), "Department_ID", "Department_Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (Category_Service.GetCategory(id) != null)
                return View(Category_Service.GetCategory(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                Category_Service.UpdateCategory(model);
                return RedirectToAction("Index");
            }
            ViewBag.Department_ID = new SelectList(department_Service.GetDepartments(), "Department_ID", "Department_Name");
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (Category_Service.GetCategory(id) != null)
                return View(Category_Service.GetCategory(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category_Service.RemoveCategory(Category_Service.GetCategory(id));
            return RedirectToAction("Index");
        }
    }
}