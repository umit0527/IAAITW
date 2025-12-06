using Ganss.Xss;
using IAAITW.Filter;
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
    [CustomAuthorize(LoginUrl = "~/Back/Admins/Login")]
    public class JobsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Jobs
        public ActionResult Index()
        {
            var data = db.Jobs.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Jobs/Details/5
        public ActionResult Details(int? id)
        {
            var data = db.Jobs.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Back/Jobs/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                job.Content = sanitizer.Sanitize(job.Content);

                // 取得登入者帳號
                var loginUser = User.Identity.Name; // ASP.NET Identity 的登入名稱
                // 比對目前登入的帳號是否存在於 Admins 資料表中
                var loginAdmin = db.Admins.FirstOrDefault(a => a.Account == loginUser);
                job.AdminId = loginAdmin.Id;
                job.UpdatedAdminId = loginAdmin.Id;
                job.CreatedDate = DateTime.Now;
                job.UpdatedDate = DateTime.Now;

                db.Jobs.Add(job);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(job);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", job.AdminId);
            return View(job);
        }

        // GET: Back/Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Back/Jobs/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                var existingJob = db.Jobs.Find(job.Id);
                if (existingJob == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingJob.Content = sanitizer.Sanitize(job.Content);

                // 取得登入者帳號
                var loginUser = User.Identity.Name; // ASP.NET Identity 的登入名稱
                // 比對目前登入的帳號是否存在於 Admins 資料表中
                var loginAdmin = db.Admins.FirstOrDefault(a => a.Account == loginUser);
                existingJob.UpdatedAdminId = loginAdmin.Id;
                existingJob.UpdatedDate = DateTime.Now;

                db.Entry(existingJob).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(job);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(job);
        }

        // POST: Back/Jobs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }
            var job = db.Jobs.Find(id);
            if (job == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Jobs.Remove(job);
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
