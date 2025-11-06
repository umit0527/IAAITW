using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Front.Controllers
{
    public class LisencesController : Controller
    {
        // GET: Front/Lisences
        public ActionResult Index()
        {
            return View();
        }

        // GET: Front/Lisences/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Front/Lisences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Front/Lisences/Create
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

        // GET: Front/Lisences/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Front/Lisences/Edit/5
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

        // GET: Front/Lisences/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Front/Lisences/Delete/5
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
