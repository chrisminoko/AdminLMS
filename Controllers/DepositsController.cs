using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BackEnd.DataManager;
using BackEnd.Models;
using Microsoft.AspNet.Identity;
namespace BackEnd.Controllers
{
    public class DepositsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Deposits

        public byte[] ConvertToBytes(HttpPostedFileBase files)
        {

            BinaryReader reader = new BinaryReader(files.InputStream);
            return reader.ReadBytes(files.ContentLength);
        }
        public ActionResult Download(int? id)
        {
            MemoryStream ms = null;

            var item = db.Institutions.FirstOrDefault(x => x.InstitutionID == id);
            if (item != null)
            {
                ms = new MemoryStream(item.FileContent);
            }
            return new FileStreamResult(ms, item.FileName);


            //return RedirectToAction("Download");
        }
        public ActionResult Index()
        {
            var deposits = db.Deposits.Include(d => d.Application);
            return View(deposits.ToList());
        }

        // GET: Deposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = db.Deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }
        public ActionResult Approve(int? id)
        {
            TempData["Message"] = "Your deposit " + id + "has been Saved Successfully ";
            Deposit deposit = db.Deposits.Where(p => p.DepositID == id).FirstOrDefault();
            int applicationID = deposit.ApplicationID;
            Application application = db.Applications.Where(a => a.ApplicationID == applicationID).FirstOrDefault();
            if (deposit.Status == "Approved" || deposit.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Th application has already been Rejected/Approved";
                return RedirectToAction("AdminIndex");
            }
            else
            {
                ApprovedDeposit approvedDeposit = new ApprovedDeposit();
                approvedDeposit.ApprovedDepositID = deposit.DepositID;
                approvedDeposit.AccountNumber = deposit.AccountNumber;
                approvedDeposit.DepositAmount = deposit.DepositAmount;
                approvedDeposit.DepositDate = deposit.DepositDate;
                approvedDeposit.AccountType = deposit.AccountType;
                approvedDeposit.FileContent = deposit.FileContent;
                approvedDeposit.FileName = deposit.FileName;
                approvedDeposit.ID = deposit.ID;
                approvedDeposit.ApplicationID = deposit.ApplicationID;
                approvedDeposit.UserEmail = deposit.UserEmail;
               
    
                application.PaymentStatus = "Approved";
                application.Status = "Active";
                application.StartDate= DateTime.Parse(DateTime.Now.ToString("yyy.MM.dd")).Date;
                application.ExpiryDateDate= DateTime.Parse(DateTime.Now.ToString("yyy.MM.dd")).Date.AddMonths(1);
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();


                approvedDeposit.Status = "Approved";
                db.ApprovedDeposits.Add(approvedDeposit);
                db.SaveChanges();


                //Send An Email After Approval with Activated User Roles and Moodle Credential
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(deposit.UserEmail, deposit.DepositAmount.ToString()));
                var body = $"Hello {deposit.UserEmail}, Congratulations. We are glad to inform you that your application has been approved. You can now procced to adding your building details. You are required to pay the Subscription Fee in order for your building to be active to the Tenants <br/> Regards,<br/><br/> HomeLink <br/> .";

                //Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                BackEnd.DataManager.EmailService emailService = new BackEnd.DataManager.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Application Statement | Ref No.:" + deposit.DepositTraceCode,
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Alliance</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()

                });


                db.Deposits.Remove(deposit);
                db.SaveChanges();

                //userManager.AddToRole(institution.UserId, "Institution");
                TempData["AlertMessage"] = $"{deposit.UserEmail} has been successfully approved";

                return RedirectToAction("Index");
            }
        }

        public decimal Pay(int id ) 
        {
           return  ViewBag.price = db.Applications.Find(id).Amount;
        
        }
        // GET: Deposits/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "UserEmail");
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepositID,NameOnAccount,ApplicationID,AccountType,AccountNumber,DepositAmount,DepositDate,UserEmail,ID,Status,DepositTraceCode,FileName,FileContent")] Deposit deposit, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    deposit.FileName = files.FileName;
                    string[] bits = deposit.FileName.Split('\\');
                    deposit.FileContent = ConvertToBytes(files);
                }

                deposit.UserEmail = User.Identity.GetUserName();
                db.Deposits.Add(deposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "UserEmail", deposit.ApplicationID);
            return View(deposit);
        }

        // GET: Deposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = db.Deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "UserEmail", deposit.ApplicationID);
            return View(deposit);
        }

        // POST: Deposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepositID,NameOnAccount,ApplicationID,AccountType,AccountNumber,DepositAmount,DepositDate,UserEmail,ID,Status,DepositTraceCode,FileName,FileContent")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "UserEmail", deposit.ApplicationID);
            return View(deposit);
        }

        // GET: Deposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = db.Deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }

        // POST: Deposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deposit deposit = db.Deposits.Find(id);
            db.Deposits.Remove(deposit);
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
