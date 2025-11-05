using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;

namespace IAAITW.Areas.Front.Controllers
{
    public class JobsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(jobs.ToList());
        }

        // GET: Front/Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Front/Jobs/Create
        public ActionResult Create()
        {
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Front/Jobs/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,CreatedDate,UpdatedDate,AdminId,UpdatedAdminId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", job.AdminId);
            ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", job.UpdatedAdminId);
            return View(job);
        }

        // GET: Front/Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", job.AdminId);
            ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", job.UpdatedAdminId);
            return View(job);
        }

        // POST: Front/Jobs/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,CreatedDate,UpdatedDate,AdminId,UpdatedAdminId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", job.AdminId);
            ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", job.UpdatedAdminId);
            return View(job);
        }

        // GET: Front/Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Front/Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
