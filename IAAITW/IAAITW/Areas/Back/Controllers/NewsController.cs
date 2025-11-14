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
    public class NewsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/News
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.LastUpdater).Include(n => n.Publisher);
            return View(news.ToList());
        }

        // GET: Back/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Back/News/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdaterId = new SelectList(db.MemberInfoes, "Id", "Name");
            ViewBag.PublisherId = new SelectList(db.MemberInfoes, "Id", "Name");
            return View();
        }

        // POST: Back/News/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 檔案上傳
                    if (news.CoverImageFile != null && news.CoverImageFile.ContentLength > 0)
                    {
                        var uploadsFolder = Server.MapPath("~/Uploads/News"); // 存放路徑

                        // 如果資料夾不存在就建立                                                  
                        if (!System.IO.Directory.Exists(uploadsFolder))
                        {
                            System.IO.Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileName = System.IO.Path.GetFileName(news.CoverImageFile.FileName);
                        var path = System.IO.Path.Combine(uploadsFolder, fileName);
                        try
                        {
                            // 儲存檔案
                            news.CoverImageFile.SaveAs(path);
                            // 設定成功訊息與跳轉網址
                            TempData["SuccessMessage"] = "新增成功！";
                            ViewBag.RedirectUrl = Url.Action("Index");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "檔案上傳失敗: " + ex.Message);
                            ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account", news.PublisherId);
                            TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
                            return View(news);
                        }
                        // 存到資料庫
                        news.CoverImage = "/Uploads/News/" + fileName; // 可存相對路徑
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "未收到檔案或檔案為空。";
                        return View(news);
                    }

                    news.CreatedDate = DateTime.Now;
                    news.UpdatedDate = DateTime.Now;

                    db.News.Add(news);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "新增成功！";
                    ViewBag.RedirectUrl = Url.Action("Index");
                    return View(news);
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
                    //TempData["DebugMessage"] = ex.Message + "\n" + ex.StackTrace;
                    return View(news);
                }
            }
            else
            {
                // 將 ModelState 錯誤加入 TempData
                var errors = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = "資料驗證失敗：" + errors;
                TempData["DebugMessage"] = "ModelState 錯誤明細：" + errors;
            }

            //ViewBag.LastUpdaterId = new SelectList(db.MemberInfoes, "Id", "Name", news.LastUpdaterId);
            //ViewBag.PublisherId = new SelectList(db.MemberInfoes, "Id", "Name", news.PublisherId);
            return View(news);
        }

        // GET: Back/News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdaterId = new SelectList(db.MemberInfoes, "Id", "Name", news.LastUpdaterId);
            ViewBag.PublisherId = new SelectList(db.MemberInfoes, "Id", "Name", news.PublisherId);
            return View(news);
        }

        // POST: Back/News/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdaterId = new SelectList(db.MemberInfoes, "Id", "Name", news.LastUpdaterId);
            ViewBag.PublisherId = new SelectList(db.MemberInfoes, "Id", "Name", news.PublisherId);
            return View(news);
        }

        // GET: Back/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Back/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
