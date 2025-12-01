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
    public class ExpertsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Experts
        public ActionResult Index()
        {
            var data = db.Experts.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Experts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }

                var data = db.Experts.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Experts/Create
        public ActionResult Create()
        {
            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Experts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expert expert)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                expert.Content = sanitizer.Sanitize(expert.Content);
                
                // 從 Session 取登入者
                var loginUser = Session["AdminLogin"] as Admin;
                expert.AdminId = loginUser.Id;
                expert.UpdatedAdminId = loginUser.Id;

                db.Experts.Add(expert);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(expert);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", expert.AdminId);
            return View(expert);
        }

        // GET: Back/Experts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            
            return View(expert);
        }

        // POST: Back/Experts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expert expert)
        {
            if (ModelState.IsValid)
            {
                var existingExpert = db.Experts.Find(expert.Id);
                if (existingExpert == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingExpert.Content = sanitizer.Sanitize(expert.Content);
                
                // 設定最後更新時間
                existingExpert.UpdatedDate = DateTime.Now;
                
                // 從 Session 取登入者
                var loginUser = Session["AdminLogin"] as Admin;
                existingExpert.UpdatedAdminId = loginUser.Id;

                db.Entry(existingExpert).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(expert);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(expert);
        }

        // POST: Back/Experts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "無效的請求" });
            }
            var expert = db.Experts.Find(id);
            if (expert == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Experts.Remove(expert);
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
