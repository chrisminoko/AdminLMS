using BackEnd.Models.OnlineShop;
using BackEnd.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackEnd.Controllers
{
    public class DepartmentsController : Controller
    {
        Department_Service department_Service;

        public DepartmentsController()
        {
            this.department_Service = new Department_Service();
        }

        public ActionResult Index()
        {
            return View(department_Service.GetDepartments());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (department_Service.GetDepartment(id) != null)
                return View(department_Service.GetDepartment(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                department_Service.AddDepartment(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (department_Service.GetDepartment(id) != null)
                return View(department_Service.GetDepartment(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                department_Service.UpdateDepartment(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (department_Service.GetDepartment(id) != null)
                return View(department_Service.GetDepartment(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            department_Service.RemoveDepartment(department_Service.GetDepartment(id));
            return RedirectToAction("Index");
        }
    }
}