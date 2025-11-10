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
    public class HistoriesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Histories
        public ActionResult Index()
        {
            var data = db.Histories.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Histories/Details/5
        public ActionResult Details(int? id)
        {
            var data = db.Histories.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Histories/Create
        public ActionResult Create()
        {
            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Histories/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(History history)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                history.Content = sanitizer.Sanitize(history.Content);

                db.Histories.Add(history);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(history);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", history.AdminId);
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", history.UpdatedAdminId);
            return View(history);
        }

        // GET: Back/Histories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", history.AdminId);
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", history.UpdatedAdminId);
            return View(history);
        }

        // POST: Back/Histories/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(History history)
        {
            if (ModelState.IsValid)
            {
                var existingHistory = db.Histories.Find(history.Id);
                if (existingHistory == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingHistory.Content = sanitizer.Sanitize(history.Content);

                // 設定最後更新時間
                existingHistory.UpdatedDate = DateTime.Now;
                existingHistory.UpdatedAdminId = history.UpdatedAdminId;

                db.Entry(existingHistory).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(history);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(history);
        }

        // POST: Back/Histories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var history = db.Histories.Find(id);
            if (history == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Histories.Remove(history);
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
