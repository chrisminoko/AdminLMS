using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;
using PayFast;
using PayFast.AspNet;
namespace BackEnd.Controllers
{

    
    public class PackagesController : Controller
    {
            
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Packages
        public ActionResult Index()
        {
            var packages = db.Packages.Include(p => p.PackageType);
            return View(packages.ToList());
        }

        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult PackagesPage() 
        {
            var packages = db.Packages.Include(p => p.PackageType);
            return View(packages.ToList());
        }

        public ActionResult PackageTypes()
        {
            var packages = db.Packages.Include(p => p.PackageType);
            return View(packages.ToList());
        }
        // GET: Packages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: Packages/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "PackageName");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageID,PackageTypeID,Storage,NumTeachers,NumStudents,NumEmails,PackageDescription,PackagePrice,UserPhoto")] Package package)
        {
            if (ModelState.IsValid)
            {
                db.Packages.Add(package);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "PackageName", package.PackageTypeID);
            return View(package);
        }

        // GET: Packages/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "PackageName", package.PackageTypeID);

            return View(package);
            //TempData["PackageID"] = id;
            //return RedirectToAction("Register", "Account");
        }

        public ActionResult Apply(int? id) 
        {
            TempData["PackageID"] = id;
            return RedirectToAction("Register", "Account");
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageID,PackageTypeID,Storage,NumTeachers,NumStudents,NumEmails,PackageDescription,PackagePrice,UserPhoto")] Package package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "PackageName", package.PackageTypeID);
            return View(package);
        }

        // GET: Packages/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }


        private readonly PayFastSettings payFastSettings;


        #region Constructor

        public PackagesController()
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

        #endregion Constructor

        #region Methods


        public ActionResult Recurring()
        {

            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            recurringRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";

            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";

            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult OnceOff(double tot)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

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
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = tot;
            onceOffRequest.item_name = "Once off option";
            onceOffRequest.item_description = "Some details about the once off payment";


            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

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

        public ActionResult OnceOff()
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault());
            //var products = db.Items.Select(x => x.Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = 1500;
            onceOffRequest.item_name = "Once off option";
            onceOffRequest.item_description = "Some details about the once off payment";


            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            return Redirect(redirectUrl);
        }

    

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            db.Packages.Remove(package);
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




        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))]PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
