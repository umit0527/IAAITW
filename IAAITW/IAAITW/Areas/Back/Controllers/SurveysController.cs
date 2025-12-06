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
    public class SurveysController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Surveys
        public ActionResult Index()
        {
            var data = db.Surveys.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Surveys/Details/5
        public ActionResult Details()
        {
            var data = db.Surveys.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Surveys/Create
        public ActionResult Create()
        {
            // 如果資料庫已經有資料，則跳轉到index，不允許再新增
            var data = db.Surveys.FirstOrDefault();
            if (data != null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Back/Surveys/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 如果需要保留顏色或字體
                survey.Content = sanitizer.Sanitize(survey.Content);

                // 取得登入者帳號
                var loginUser = User.Identity.Name; // ASP.NET Identity 的登入名稱
                // 比對目前登入的帳號是否存在於 Admins 資料表中
                var loginAdmin = db.Admins.FirstOrDefault(a => a.Account == loginUser);
                survey.AdminId = loginAdmin.Id;
                survey.UpdatedAdminId = loginAdmin.Id;
                survey.CreatedDate = DateTime.Now;
                survey.UpdatedDate = DateTime.Now;

                db.Surveys.Add(survey);
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "新增成功！";
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(survey);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", survey.AdminId);
            return View(survey);
        }

        // GET: Back/Surveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return RedirectToAction("Error500", "Error");
            }
            return View(survey);
        }

        // POST: Back/Surveys/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Survey survey)
        {
            if (ModelState.IsValid)
            {
                var existingSurvey = db.Surveys.Find(survey.Id);
                if (existingSurvey == null)
                {
                    return RedirectToAction("Error500", "Error");
                }

                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingSurvey.Content = sanitizer.Sanitize(survey.Content);

                // 取得登入者帳號
                var loginUser = User.Identity.Name; // ASP.NET Identity 的登入名稱
                // 比對目前登入的帳號是否存在於 Admins 資料表中
                var loginAdmin = db.Admins.FirstOrDefault(a => a.Account == loginUser);
                existingSurvey.UpdatedAdminId = loginAdmin.Id;
                existingSurvey.UpdatedDate = DateTime.Now;

                db.Entry(existingSurvey).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(survey);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", survey.AdminId);
            return View(survey);
        }

        // POST: Back/Surveys/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            var survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Surveys.Remove(survey);
            db.SaveChanges();

            return Json(new { success = true });
        }    
    }
}
