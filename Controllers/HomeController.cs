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
        private Order_Service order_Service = new Order_Service();
        Item_Service item_Service = new Item_Service();
        Department_Service department_Service = new Department_Service();
        public ActionResult Index(int? id)
        {


            var items_results = new List<Item>();
            try
            {
                if (id != null)
                {
                    var firstImage = (from i in db.displayImages
                                      where i.Id == 1
                                      select i.firstImage).FirstOrDefault().ToString();
                    ViewBag.FirstImage = firstImage;

                    var secondImage = (from i in db.displayImages
                                       where i.Id == 1
                                       select i.SecondImage).FirstOrDefault().ToString();
                    ViewBag.SecondImage = secondImage;

                    if (id == 0)
                    {
                        items_results = item_Service.GetItems();
                        ViewBag.Department = "All Departments";
                    }
                    else
                    {
                        items_results = item_Service.GetItems().Where(x => x.Category.Department_ID == (int)id).ToList();
                        ViewBag.Department = department_Service.GetDepartment(id).Department_Name;
                    }
                }
                else
                {
                    items_results = item_Service.GetItems();
                    ViewBag.Department = "All Departments";
                    var firstImage = (from i in db.displayImages
                                      where i.Id == 1
                                      select i.firstImage).FirstOrDefault().ToString();
                    ViewBag.FirstImage = firstImage;

                    var secondImage = (from i in db.displayImages
                                       where i.Id == 1
                                       select i.SecondImage).FirstOrDefault().ToString();
                    ViewBag.SecondImage = secondImage;
                }
            }
            catch (Exception ex) { }
            return View(items_results);
        }
        public ActionResult AutoCalculatedBarWidth()
        {
            return View();
        }
        [Authorize]
        public ActionResult CRM()
        {
             var amount= (from i in db.Applications
                          where i.PaymentStatus == "Approved"
                          select i.Amount).Sum();

                         
                        
            double total = 0;
            var totalCustomer = (from c in db.Customers
                                 select c.Email).Count();
            var totalProducts = (from p in db.Items
                                 select p.ItemCode).Count();
            ViewBag.Products = totalProducts;
            var lowstock = (from c in db.Items
                            where c.QuantityInStock < 10
                            select c.ItemCode).Count();

            ViewBag.lowstock = lowstock;
            ViewBag.Allcustomers = totalCustomer;


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

        [Authorize]
        public ActionResult PendingApplications() 
        {

            return View(db.Institutions.ToList());
        }
        [Authorize]
        public ActionResult ShopCustomers()
        {

            return View(db.Customers.ToList());
        }
        [Authorize]
        public ActionResult PendingDeposit()
        {

            return View(db.Deposits.ToList());
        }
        [Authorize]
        public ActionResult ShopTracking(string id)
        {

            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.Status = "All";
                return View(order_Service.GetOrders());
            }
            else
            {
                ViewBag.Status = id;
                return View(order_Service.GetOrders(id));
            }
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