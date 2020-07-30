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
    public class IndividualsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Individuals
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Tenant"))
            {
                return View(db.Individuals.ToList().Where(x => x.Email == userName));

            }
            else
            {
                return View(db.Individuals.ToList());

            }
        }

        // GET: Individuals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Individual individual = db.Individuals.Find(id);
            if (individual == null)
            {
                return HttpNotFound();
            }
            return View(individual);
        }

        // GET: Individuals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Individuals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IndividualID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] Individual individual)
        {
            if (ModelState.IsValid)
            {
                db.Individuals.Add(individual);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(individual);
        }

        // GET: Individuals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Individual individual = db.Individuals.Find(id);
            if (individual == null)
            {
                return HttpNotFound();
            }
            return View(individual);
        }

        // POST: Individuals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IndividualID,FullName,Type,Status,Email,Phone,UserId,FileName,FileContent,UserPhoto")] Individual individual)
        {
            if (ModelState.IsValid)
            {
                db.Entry(individual).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(individual);
        }

        // GET: Individuals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Individual individual = db.Individuals.Find(id);
            if (individual == null)
            {
                return HttpNotFound();
            }
            return View(individual);
        }

        // POST: Individuals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Individual individual = db.Individuals.Find(id);
            db.Individuals.Remove(individual);
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
