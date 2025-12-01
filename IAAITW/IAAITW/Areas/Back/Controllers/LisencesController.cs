using Ganss.Xss;
using IAAITW.Filter;
using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Back.Controllers
{
    [CustomAuthorize(LoginUrl = "~/Back/Admins/Login")]
    public class LisencesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Lisences/Index
        public ActionResult Index()
        {
            var data = db.Lisences.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Lisences/Details/5
        public ActionResult Details()
        {
            var data = db.Lisences.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Lisences/Create
        public ActionResult Create()
        {
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Lisences/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lisences lisences)
        {
            // 從 Session 取登入者
            var loginUser = Session["AdminLogin"] as Admin;
            if (loginUser == null)
            {
                // 設定錯誤訊息與跳轉網址
                TempData["LoginMessage"] = "請先登入！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Login", "Admins");

                return View(lisences);
            }

            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 如果需要保留顏色或字體
                lisences.Content = sanitizer.Sanitize(lisences.Content);

                lisences.AdminId = loginUser.Id;
                lisences.UpdatedAdminId = loginUser.Id;
                lisences.CreatedDate = DateTime.Now;
                lisences.UpdatedDate = DateTime.Now;

                db.Lisences.Add(lisences);
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "新增成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(lisences);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            return View(lisences);
        }

        // GET: Back/Lisences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            Lisences lisences = db.Lisences.Find(id);
            if (lisences == null)
            {
                return HttpNotFound();
            }

            return View(lisences);
        }

        // POST: Back/Lisences/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lisences lisences)
        {
            if (ModelState.IsValid)
            {
                var existingLisences = db.Lisences.Find(lisences.Id);
                if (existingLisences == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingLisences.Content = sanitizer.Sanitize(lisences.Content);

                // 設定最後更新時間
                existingLisences.UpdatedDate = DateTime.Now;
                // 從 Session 取登入者
                var loginUser = Session["AdminLogin"] as Admin;
                // 更新編輯者
                existingLisences.UpdatedAdminId = loginUser.Id;

                db.Entry(existingLisences).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(lisences);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(lisences);
        }

        // POST: Back/Lisences/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "無效的請求" });
            }
            var lisences = db.Lisences.Find(id);
            if (lisences == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Lisences.Remove(lisences);
            db.SaveChanges();

            return Json(new { success = true });
        }
    }
}
