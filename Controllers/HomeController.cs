using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using BackEnd.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace BackEnd.Controllers
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Age { get; set; }
    }
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Order_Service _order_Service = new Order_Service();


        // GET: Home/JqAJAX  
        [HttpPost]
        [Route("JqAJAXs")]
        public ActionResult JqAJAX(Student st)
        {
            try
            {
                return Json(new
                {
                    msg = "Successfully added " + st.Name
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CRM()
        {
             var amount= (from i in db.Applications
                          where i.PaymentStatus == "Approved"
                          select i.Amount).Sum();

                         
                        
            double total = 0;
           
            foreach (var item in db.Order_Items) 
            {
                 total += item.price * item.quantity;
            
            }
            ViewBag.totalShopSales = total;
            if (amount==0) 
            {
                ViewBag.TotalSales = 0;
            }
            else
            {
                ViewBag.TotalSales = (from i in db.Applications
                                      where i.PaymentStatus == "Approved"
                                      select i.Amount).Sum();
            }

            ViewBag.TotalStudents = db.Packages.Count();

            return View();

        }
    
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ConfirmEmail()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PendingApplications() 
        {

            return View(db.Institutions.ToList());
        }
      
        public PartialViewResult Graph() 
        {
            return PartialView("_PendingApplications");
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
        public FileContentResult UserPhotos()
        {

            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    return File(imageData, "image/png");
                }
              
                // to get the user details to load user Image    
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userImage.UserPhoto == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    return File(imageData, "image/png");
                }
                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
        
            else 
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            } 
        }
    }
}