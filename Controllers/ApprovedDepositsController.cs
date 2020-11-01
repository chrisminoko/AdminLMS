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
    public class ApprovedDepositsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApprovedDeposits
        public ActionResult Index()
        {
            if (User.IsInRole("Admin")) 
            {
                return View(db.ApprovedDeposits.ToList());
            }
            else 
            {
                return View(db.ApprovedDeposits.ToList().Where(x=>x.UserEmail.Equals(User.Identity.GetUserName())));
            }
         
        }

        // GET: ApprovedDeposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedDeposit approvedDeposit = db.ApprovedDeposits.Find(id);
            if (approvedDeposit == null)
            {
                return HttpNotFound();
            }
            return View(approvedDeposit);
        }

        // GET: ApprovedDeposits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApprovedDeposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApprovedDepositID,NameOnAccount,AccountType,AccountNumber,ApplicationID,DepositAmount,DepositDate,UserEmail,ID,Status,DepositTraceCode,FileName,FileContent")] ApprovedDeposit approvedDeposit)
        {
            if (ModelState.IsValid)
            {
                db.ApprovedDeposits.Add(approvedDeposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(approvedDeposit);
        }

        // GET: ApprovedDeposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedDeposit approvedDeposit = db.ApprovedDeposits.Find(id);
            if (approvedDeposit == null)
            {
                return HttpNotFound();
            }
            return View(approvedDeposit);
        }

        // POST: ApprovedDeposits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApprovedDepositID,NameOnAccount,AccountType,AccountNumber,ApplicationID,DepositAmount,DepositDate,UserEmail,ID,Status,DepositTraceCode,FileName,FileContent")] ApprovedDeposit approvedDeposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approvedDeposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(approvedDeposit);
        }

        // GET: ApprovedDeposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedDeposit approvedDeposit = db.ApprovedDeposits.Find(id);
            if (approvedDeposit == null)
            {
                return HttpNotFound();
            }
            return View(approvedDeposit);
        }

        // POST: ApprovedDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApprovedDeposit approvedDeposit = db.ApprovedDeposits.Find(id);
            db.ApprovedDeposits.Remove(approvedDeposit);
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
