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
    public class InstitutionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        public ActionResult UserIndex()
        {
            var userName = User.Identity.GetUserName();
            var institutions = db.Institutions.Where(x => x.Email == userName);
            return View(institutions.ToList());
        }
        // GET: Institutions
        public ActionResult Index()
        {
            return View(db.Institutions.ToList());
        }

        public ActionResult Approve(int? id)
        {
            Institution institution = db.Institutions.Where(p => p.InstitutionID == id).FirstOrDefault();
            if (institution.Status == "Approved" || institution.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Th application has already been Rejected/Approved";
                return RedirectToAction("AdminIndex");
            }
            else
            {
                ApprovedInstitution approvedInstitution = new ApprovedInstitution();
                approvedInstitution.InstitutionID = institution.InstitutionID;
                approvedInstitution.FullName = institution.FullName;

                approvedInstitution.Email = institution.Email;

                approvedInstitution.Phone = institution.Phone;
                approvedInstitution.Type = institution.Type;
                approvedInstitution.FileName = institution.FileName;
                approvedInstitution.UserPhoto = institution.UserPhoto;

                approvedInstitution.Status = "Approved";
                db.ApprovedInstitutions.Add(approvedInstitution);
                db.SaveChanges();
               

                //Send An Email After Approval with Activated User Roles and Moodle Credential
                
       

                db.Institutions.Remove(institution);
                db.SaveChanges();
                
                //userManager.AddToRole(institution.UserId, "Institution");
                TempData["AlertMessage"] = $"{institution.FullName} has been successfully approved";

                return RedirectToAction("Index");
            }
        }
        public ActionResult Reject(int? id)
        {
            Institution institution = db.Institutions.Where(p => p.InstitutionID == id).FirstOrDefault();
            if (institution.Status == "Rejected" || institution.Status == "Approved")
            {
                TempData["AlertMessage"] = "Th application has already been Rejected";
                return RedirectToAction("AdminIndex");
            }
            else
            {
                institution.Status = "Rejected";
                db.Entry(institution).State = EntityState.Modified;
                db.SaveChanges();

                TempData["AlertMessage"] = $"{institution.FullName} has been rejected";

                return RedirectToAction("AdminIndex");
            }
        }

        // GET: Institutions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        // GET: Institutions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstitutionID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Institutions.Add(institution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(institution);
        }

        // GET: Institutions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        // POST: Institutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstitutionID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(institution);
        }

        // GET: Institutions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        // POST: Institutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Institution institution = db.Institutions.Find(id);
            db.Institutions.Remove(institution);
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
