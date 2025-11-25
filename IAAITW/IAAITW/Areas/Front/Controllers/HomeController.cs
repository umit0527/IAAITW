using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;
//using static IAAITW.Models.News;

namespace IAAITW.Areas.Front.Controllers
{
    public class HomeController : Controller
    {
        DBMdoelContext db = new DBMdoelContext();
        // GET: Front/Home
        public ActionResult Index()
        {
            var newsList = db.News.ToList(); 
            return View(newsList);
        }

        // GET: Front/Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Front/Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Front/Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Front/Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Front/Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Front/Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Front/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
