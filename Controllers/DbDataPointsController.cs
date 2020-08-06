using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;
using Newtonsoft.Json;

namespace BackEnd.Controllers
{
    public class DbDataPointsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DbDataPoints
        public ActionResult Index()
        {
            InformationRequestHub.NotifyInformationRequestToClient();
            return View(db.dbDataPoints.ToList());
        }
       


        public ActionResult BarGraph()
        {
            try
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(db.dbDataPoints.ToList(), _jsonSetting);

                return View();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    


    // GET: DbDataPoints/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbDataPoint dbDataPoint = db.dbDataPoints.Find(id);
            if (dbDataPoint == null)
            {
                return HttpNotFound();
            }
            return View(dbDataPoint);
        }

        // GET: DbDataPoints/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DbDataPoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,x,y")] DbDataPoint dbDataPoint)
        {
            if (ModelState.IsValid)
            {
                db.dbDataPoints.Add(dbDataPoint);
                db.SaveChanges();
                InformationRequestHub.NotifyInformationRequestToClient();
                return RedirectToAction("Index");
            }

            return View(dbDataPoint);
        }

        // GET: DbDataPoints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbDataPoint dbDataPoint = db.dbDataPoints.Find(id);
            if (dbDataPoint == null)
            {
                return HttpNotFound();
            }
            return View(dbDataPoint);
        }

        // POST: DbDataPoints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,x,y")] DbDataPoint dbDataPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbDataPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dbDataPoint);
        }

        // GET: DbDataPoints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbDataPoint dbDataPoint = db.dbDataPoints.Find(id);
            if (dbDataPoint == null)
            {
                return HttpNotFound();
            }
            return View(dbDataPoint);
        }

        // POST: DbDataPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbDataPoint dbDataPoint = db.dbDataPoints.Find(id);
            db.dbDataPoints.Remove(dbDataPoint);
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
