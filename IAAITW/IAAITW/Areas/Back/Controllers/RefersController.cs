using Ganss.Xss;
using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Back.Controllers
{
    public class RefersController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Refers
        public ActionResult Index()
        {
            var data = db.Refers.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Refers/Details/5
        public ActionResult Details(int? id)
        {
            var data = db.Refers.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Refers/Create
        public ActionResult Create()
        {
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Refers/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Refer refer)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 如果需要保留顏色或字體
                refer.Content = sanitizer.Sanitize(refer.Content);

                db.Refers.Add(refer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", refer.AdminId);
            return View(refer);
        }

        // GET: Back/Refers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Refer refer = db.Refers.Find(id);
            if (refer == null)
            {
                return HttpNotFound();
            }
            ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", refer.UpdatedAdminId);
            return View(refer);
        }

        // POST: Back/Refers/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Refer refer)
        {
            if (ModelState.IsValid)
            {
                var existingRefer = db.Refers.Find(refer.Id);
                if (existingRefer == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingRefer.Content = sanitizer.Sanitize(refer.Content);

                // 設定最後更新時間
                existingRefer.UpdatedDate = DateTime.Now;

                db.Entry(existingRefer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", refer.AdminId);
            return View(refer);
        }

        // GET: Back/Refers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Refer refer = db.Refers.Find(id);
            if (refer == null)
            {
                return HttpNotFound();
            }
            return View(refer);
        }

        // POST: Back/Refers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var refer = db.Refers.Find(id);
            if (refer == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Refers.Remove(refer);
            db.SaveChanges();

            return Json(new { success = true });
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
