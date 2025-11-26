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
    [Authorize]
    public class AboutsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Abouts/Index
        public ActionResult Index()
        {
            var data = db.Abouts.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Abouts/Details/5
        public ActionResult Details(int? id)
        {
            var data = db.Abouts.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Abouts/Create
        public ActionResult Create()
        {
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Abouts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(About about)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 如果需要保留顏色或字體
                about.Content = sanitizer.Sanitize(about.Content);

                db.Abouts.Add(about);
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "新增成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(about);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", about.AdminId);
            return View(about);
        }

        // GET: Back/Abouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            
            return View(about);
        }

        // POST: Back/Abouts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(About about)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.Find(about.Id);
                if (existingAbout == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingAbout.Content = sanitizer.Sanitize(about.Content);

                // 設定最後更新時間
                existingAbout.UpdatedDate = DateTime.Now;
                existingAbout.UpdatedAdminId=about.UpdatedAdminId;

                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(about);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(about);
        }

        // GET: Back/Abouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Back/Abouts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var about = db.Abouts.Find(id);
            if (about == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Abouts.Remove(about);
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
