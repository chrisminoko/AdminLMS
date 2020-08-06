using BackEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SweetAlert.Controllers
{
    public class SweetController : Controller
    {
        // GET: Sweet
        public ActionResult Index()
        {
			List<DataPoint> dataPoints = new List<DataPoint>();

			dataPoints.Add(new DataPoint(1, 0));
			dataPoints.Add(new DataPoint(2, 0));
			dataPoints.Add(new DataPoint(3, 31));
			dataPoints.Add(new DataPoint(4, 145));
			dataPoints.Add(new DataPoint(5, 341));
			dataPoints.Add(new DataPoint(6, 620));
			dataPoints.Add(new DataPoint(7, 1007));
			dataPoints.Add(new DataPoint(8, 1486));
			dataPoints.Add(new DataPoint(9, 1857));
			dataPoints.Add(new DataPoint(10, 1983));
			dataPoints.Add(new DataPoint(11, 1994));
			dataPoints.Add(new DataPoint(12, 1998));
			dataPoints.Add(new DataPoint(13, 2000));
			dataPoints.Add(new DataPoint(14, 2000));
			dataPoints.Add(new DataPoint(15, 2000));
			dataPoints.Add(new DataPoint(16, 2000));
			dataPoints.Add(new DataPoint(17, 2000));
			dataPoints.Add(new DataPoint(18, 2000));
			dataPoints.Add(new DataPoint(19, 2000));
			dataPoints.Add(new DataPoint(20, 2000));
			dataPoints.Add(new DataPoint(21, 2000));
			dataPoints.Add(new DataPoint(22, 1906));
			dataPoints.Add(new DataPoint(23, 1681));
			dataPoints.Add(new DataPoint(24, 1455));
			dataPoints.Add(new DataPoint(25, 1230));

			ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

			return View();
		}
    }
}