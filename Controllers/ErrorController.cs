using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackEnd.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Bad_Request()
        {
            return View();
        }
        public ActionResult Not_Found()
        {
            return View();
        }
    }
}