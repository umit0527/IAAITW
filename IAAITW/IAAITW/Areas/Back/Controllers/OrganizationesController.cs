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
    public class OrganizationesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Organizations
        public ActionResult Index()
        {
            var data = db.Organizationes.FirstOrDefault();

            // 若沒有資料，導向 Details（會讓它顯示「Create New」）
            if (data == null)
            {
                return RedirectToAction("Details"); // 不帶 id，讓 Details 自行判斷
            }
            //導轉到 Details
            return RedirectToAction("Details", new { id = data.Id });
        }

        // GET: Back/Organizations/Details/5
        public ActionResult Details(int? id)
        {
            var data = db.Organizationes.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);
        }

        // GET: Back/Organizations/Create
        public ActionResult Create()
        {
            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account");
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Organizations/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                organization.Content = sanitizer.Sanitize(organization.Content);

                db.Organizationes.Add(organization);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Details", new { id = organization.Id });

                return View(organization);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", organization.AdminId);
            return View(organization);
        }

        // GET: Back/Organizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizationes.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", organization.AdminId);
            //ViewBag.UpdatedAdminId = new SelectList(db.Admins, "Id", "Account", organization.UpdatedAdminId);
            return View(organization);
        }

        // POST: Back/Organizations/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Organization organization)
        {
            if (ModelState.IsValid)
            {
                var existingOrganization = db.Organizationes.Find(organization.Id);
                if (existingOrganization == null)
                {
                    return HttpNotFound();
                }
                // 安全過濾 HTML 並更新內容
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                existingOrganization.Content = sanitizer.Sanitize(organization.Content);

                // 設定最後更新時間
                existingOrganization.UpdatedDate = DateTime.Now;
                existingOrganization.UpdatedAdminId = organization.UpdatedAdminId;

                db.Entry(existingOrganization).State = EntityState.Modified;
                db.SaveChanges();

                // 設定成功訊息與跳轉網址
                TempData["SuccessMessage"] = "編輯成功！";
                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(organization);
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            return View(organization);
        }

        // POST: Back/Organizations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var organization = db.Organizationes.Find(id);
            if (organization == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            db.Organizationes.Remove(organization);
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
