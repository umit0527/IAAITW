using Ganss.Xss;
using IAAITW.Models;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IAAITW.Areas.Back.Controllers
{
    public class NewsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/News
        public ActionResult Index(int? page)
        {
            //一頁幾筆資料
            var pageSize = 10;

            //目前第幾頁
            ///避免page是null的時候
            ///page-1是為了與後端的值對齊
            ///當前端是第一頁 value=1、後端value應該要是0，從0開始計算
            if (page.HasValue)
            {
                page = page - 1;
            }
            else
            {
                page = 0;
            }

            //用套件一定要有 orderby 排序
            var result = db.News.OrderByDescending(x => x.IsPinned)
                                   .ThenByDescending(x => x.UpdatedDate)
                                   .ToPagedList(page.Value, pageSize);

            //var news = db.News.Include(n => n.LastUpdater).Include(n => n.Publisher);
            return View(result);
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
                        // 允許的副檔名
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(news.CoverImageFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            TempData["ErrorMessage"] = "檔案格式不正確，請上傳 JPG、JPEG、PNG或GIF";
                            return View(news);
                        }

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
                        TempData["ErrorMessage"] = "請上傳封面圖片";
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

            var news = db.News.Include(k => k.Publisher)
                              .FirstOrDefault(k => k.Id == id);
            if (news == null)
            {
                return HttpNotFound();
            }

            var viewModel = new News
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                CoverImage = news.CoverImage,
                IsPinned = news.IsPinned,
                LastUpdaterId = news.LastUpdaterId,
            };

            //ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account", news.PublisherId);
            return View(viewModel);
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
                try
                {
                    var existingNews = db.News
                        .Include(k => k.LastUpdater)
                        .FirstOrDefault(k => k.Id == news.Id);

                    if (existingNews == null)
                    {
                        return HttpNotFound();
                    }

                    // 更新基本欄位
                    existingNews.Title = news.Title;
                    existingNews.Content = news.Content;
                    existingNews.IsPinned = news.IsPinned;
                    existingNews.LastUpdaterId = news.LastUpdaterId;

                    // 處理封面上傳
                    if (news.CoverImageFile != null && news.CoverImageFile.ContentLength > 0)
                    {
                        // 允許的副檔名
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(news.CoverImageFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            TempData["ErrorMessage"] = "檔案格式不正確，請上傳 JPG、JPEG、PNG或GIF";
                            return View(news);
                        }

                        var uploadsFolder = Server.MapPath("~/Uploads/News");
                        if (!System.IO.Directory.Exists(uploadsFolder))
                            System.IO.Directory.CreateDirectory(uploadsFolder);

                        // 刪掉舊檔案
                        if (!string.IsNullOrEmpty(existingNews.CoverImage))
                        {
                            var oldFile = Server.MapPath(existingNews.CoverImage);
                            if (System.IO.File.Exists(oldFile))
                                System.IO.File.Delete(oldFile);
                        }

                        // 儲存新檔案
                        var fileName = System.IO.Path.GetFileName(news.CoverImageFile.FileName);
                        var path = System.IO.Path.Combine(uploadsFolder, fileName);
                        news.CoverImageFile.SaveAs(path);
                        existingNews.CoverImage = "/Uploads/News/" + fileName;
                    }

                    // 更新修改時間
                    existingNews.UpdatedDate = DateTime.Now;

                    db.Entry(existingNews).State = EntityState.Modified;
                    db.SaveChanges();

                    // 設定成功訊息與跳轉網址
                    TempData["SuccessMessage"] = "編輯成功！";
                    ViewBag.RedirectUrl = Url.Action("Index");
                    return View(news);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "更新失敗: " + ex.Message);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "編輯失敗，請檢查輸入的資料。";
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", news.LastUpdaterId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            // 刪除實體檔案
            if (!string.IsNullOrEmpty(news.CoverImage))
            {
                var filePath = Server.MapPath(news.CoverImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.News.Remove(news);
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
