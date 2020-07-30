using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;
using Microsoft.AspNet.Identity;

namespace BackEnd.Controllers
{
    public class ApprovedInstitutionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApprovedInstitutions
        public ActionResult Index()
        {
            var email = User.Identity.GetUserName();
            return View(db.ApprovedInstitutions.ToList().Where(p => p.Email == email)); 
        }

        public ActionResult Approve(int? id)
        {
            ApprovedInstitution approvedOwners = db.ApprovedInstitutions.Where(p => p.InstitutionID == id).FirstOrDefault();
            approvedOwners.Status = "Paid";
            db.Entry(approvedOwners).State = EntityState.Modified;
            db.SaveChanges();
            TempData["AlertMessage"] = $"Status successfully updated";

            return RedirectToAction("Index1");
        }

        // GET: ApprovedInstitutions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedInstitution approvedInstitution = db.ApprovedInstitutions.Find(id);
            if (approvedInstitution == null)
            {
                return HttpNotFound();
            }
            return View(approvedInstitution);
        }

        // GET: ApprovedInstitutions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApprovedInstitutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstitutionID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] ApprovedInstitution approvedInstitution)
        {
            if (ModelState.IsValid)
            {
                db.ApprovedInstitutions.Add(approvedInstitution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(approvedInstitution);
        }

        // GET: ApprovedInstitutions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedInstitution approvedInstitution = db.ApprovedInstitutions.Find(id);
            if (approvedInstitution == null)
            {
                return HttpNotFound();
            }
            return View(approvedInstitution);
        }

        // POST: ApprovedInstitutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstitutionID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] ApprovedInstitution approvedInstitution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approvedInstitution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(approvedInstitution);
        }

        // GET: ApprovedInstitutions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedInstitution approvedInstitution = db.ApprovedInstitutions.Find(id);
            if (approvedInstitution == null)
            {
                return HttpNotFound();
            }
            return View(approvedInstitution);
        }

        // POST: ApprovedInstitutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApprovedInstitution approvedInstitution = db.ApprovedInstitutions.Find(id);
            db.ApprovedInstitutions.Remove(approvedInstitution);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
