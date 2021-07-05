using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    public class DisplayImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DisplayImages
        public ActionResult Index()
        {
            return View(db.displayImages.ToList());
        }

        // GET: DisplayImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImages displayImages = db.displayImages.Find(id);
            if (displayImages == null)
            {
                return HttpNotFound();
            }
            return View(displayImages);
        }

        // GET: DisplayImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisplayImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,firstImage,SecondImage,thirdImage,fouthImage")] DisplayImages displayImages)
        {
            if (ModelState.IsValid)
            {
                db.displayImages.Add(displayImages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayImages);
        }

        // GET: DisplayImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImages displayImages = db.displayImages.Find(id);
            if (displayImages == null)
            {
                return HttpNotFound();
            }
            return View(displayImages);
        }

        // POST: DisplayImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstImage,SecondImage,thirdImage,fouthImage")] DisplayImages displayImages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(displayImages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayImages);
        }

        // GET: DisplayImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImages displayImages = db.displayImages.Find(id);
            if (displayImages == null)
            {
                return HttpNotFound();
            }
            return View(displayImages);
        }

        // POST: DisplayImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisplayImages displayImages = db.displayImages.Find(id);
            db.displayImages.Remove(displayImages);
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
