using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;

using PayFast;
using PayFast.AspNet;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace BackEnd.Controllers
{
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Package);
            return View(applications.ToList());
        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create()
        {
            ViewBag.PackageID = new SelectList(db.Packages, "PackageID", "Storage");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationID,PackageID,UserEmail,ApplicationDate,StartDate,ExpiryDateDate,Status,PaymentStatus,Amount")] Application application)
        {
            if (ModelState.IsValid)
            {
                application.UserEmail = User.Identity.GetUserName();
                application.ApplicationDate = DateTime.Parse(DateTime.Now.ToString("yyy.MM.dd")).Date;
                application.Status = "Inactive";
                application.PaymentStatus = "Awaiting Payment";


                decimal amount = (from p in db.Packages
                                  where p.PackageID == application.PackageID
                                  select p.PackagePrice).FirstOrDefault();
                application.Amount = amount;

                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageID = new SelectList(db.Packages, "PackageID", "Storage", application.PackageID);
            return View(application);
        }

        public ActionResult Pay()
        {
            return null;
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageID = new SelectList(db.Packages, "PackageID", "Storage", application.PackageID);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationID,PackageID,UserEmail,ApplicationDate,StartDate,ExpiryDateDate,Status,PaymentStatus,Amount")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageID = new SelectList(db.Packages, "PackageID", "Storage", application.PackageID);
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        private readonly PayFastSettings payFastSettings;


        #region Constructor

        public ApplicationsController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult OnceOff(int id)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            Application application = db.Applications.Where(a => a.ApplicationID == id).FirstOrDefault();
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            double amount = 20/* Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault())*/;
            var products = "Gold" /*db.Items.Select(x => x.Name).ToList()*/;
            // Transaction Details
            decimal? PackagePrice = (from p in db.Applications
                                     where p.ApplicationID == id
                                     select p.Amount).FirstOrDefault();

            var PackageName = (from p in db.Applications
                               where p.ApplicationID == id
                               select p.Package.PackageType.PackageName).FirstOrDefault();

            var Description = (from p in db.Applications
                               where p.ApplicationID == id
                               select p.Package.PackageDescription).FirstOrDefault();

            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(PackagePrice);
            onceOffRequest.item_name = PackageName;
            onceOffRequest.item_description = Description;


            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";
            application.PaymentStatus = "Approved";
            application.Status = "Active";
            application.StartDate = DateTime.Parse(DateTime.Now.ToString("yyy.MM.dd")).Date;
            application.ExpiryDateDate = DateTime.Parse(DateTime.Now.ToString("yyy.MM.dd")).Date.AddMonths(1);
            db.Entry(application).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(redirectUrl);
        }

        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;
            #endregion Methods
            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";
            double amount = 70; /*Convert.ToDouble(db.FoodOrders.Select(x => x.Total).FirstOrDefault());*/
            var products = "Gold"/* db.FoodOrders.Select(x => x.UserEmail).ToList()*/;
            // Transaction Details


            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
