
using BackEnd.Models.OnlineShop;
using BackEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackEnd.Controllers
{
    public class ItemsController : Controller
    {
        private Item_Service item;
        Category_Service category_Service;
        public ItemsController()
        {
            this.item = new Item_Service();
            this.category_Service = new Category_Service();
        }
        public ActionResult Index()
        {
            return View(item.GetItems());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (item.GetItem(id) != null)
                return View(item.GetItem(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(category_Service.GetCategories(), "Category_ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item model, HttpPostedFileBase photoUpload)
        {
            ViewBag.Category_ID = new SelectList(category_Service.GetCategories(), "Category_ID", "Name");
            byte[] photo = null;
            photo = new byte[photoUpload.ContentLength];
            photoUpload.InputStream.Read(photo, 0, photoUpload.ContentLength);
            model.Picture = photo;

            if (ModelState.IsValid)
            {
                item.AddItem(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Category_ID = new SelectList(category_Service.GetCategories(), "Category_ID", "Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (item.GetItem(id) != null)
                return View(item.GetItem(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model, HttpPostedFileBase img_upload)
        {
            byte[] Models = null;
            Models = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(Models, 0, img_upload.ContentLength);
            model.Picture = Models;
            if (ModelState.IsValid)
            {
                item.UpdateItem(model);
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(category_Service.GetCategories(), "Category_ID", "Name");
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (item.GetItem(id) != null)
                return View(item.GetItem(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item.RemoveItem(item.GetItem(id));
            return RedirectToAction("Index");
        }
    }
}