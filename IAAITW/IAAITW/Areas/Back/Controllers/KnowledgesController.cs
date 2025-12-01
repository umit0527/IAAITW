using IAAITW.Filter;
using IAAITW.Models;
using MvcPaging;
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
    public class KnowledgesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Knowledges
        public ActionResult Index(int? page)
        {
            //指令化
            var knowledges = db.Knowledges.AsQueryable();

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
            var result = knowledges.OrderByDescending(x => x.IsTop)       
                                   .ThenByDescending(x => x.UploadDate)
                                   .ToPagedList(page.Value, pageSize);

            //var knowledges = db.Knowledges.Include(k => k.Admin);
            return View(result);
        }

        // GET: Back/Knowledges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // GET: Back/Knowledges/Create
        public ActionResult Create()
        {
            ViewBag.UploadUserId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Back/Knowledges/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KnowledgeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var knowledge = new Knowledge
                {
                    Title = model.Title,
                    //Description = model.Description,
                    IsTop= model.IsTop,
                    FilePath = model.FilePath,
                    AdminId = model.AdminId,
                    UploadDate = model.UploadDate,
                    UpdatedDate = model.UpdatedDate,
                };

                // 檔案上傳
                if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
                {
                    var uploadsFolder = Server.MapPath("~/Uploads/Knowledge"); // 存放路徑

                    // 如果資料夾不存在就建立                                                  
                    if (!System.IO.Directory.Exists(uploadsFolder))
                    {
                        System.IO.Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = System.IO.Path.GetFileName(model.FileUpload.FileName);
                    var path = System.IO.Path.Combine(uploadsFolder, fileName);
                    try
                    {
                        // 儲存檔案
                        model.FileUpload.SaveAs(path);
                        // 設定成功訊息與跳轉網址
                        TempData["SuccessMessage"] = "新增成功！";
                        ViewBag.RedirectUrl = Url.Action("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "檔案上傳失敗: " + ex.Message);
                        ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", model.AdminId);
                        TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
                        return View(model);
                    }
                    // 存到資料庫
                    knowledge.FilePath = "/Uploads/Knowledge/" + fileName; // 可存相對路徑
                }
                else
                {
                    TempData["ErrorMessage"] = "請上傳檔案";
                }

                db.Knowledges.Add(knowledge);
                db.SaveChanges();
                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
                //ModelState.AddModelError("", "未收到檔案或檔案為空");
            }

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", model.AdminId);
            return View(model);
        }

        // GET: Back/Knowledges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            var knowledge = db.Knowledges
                              .Include(k => k.Admin)
                              .FirstOrDefault(k => k.Id == id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }

            // 將 Knowledge 轉換為 KnowledgeViewModel
            var viewModel = new KnowledgeViewModel
            {
                Id = knowledge.Id,
                Title = knowledge.Title,
                Description = knowledge.Description,
                IsTop = knowledge.IsTop,
                FilePath = knowledge.FilePath,
                AdminId = knowledge.AdminId,
                UploadDate = knowledge.UploadDate,
                Admin = knowledge.Admin,
            };

            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", knowledge.AdminId);
            return View(viewModel);
        }

        // POST: Back/Knowledges/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KnowledgeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingKnowledge = db.Knowledges
                        .Include(k => k.Admin)
                        .FirstOrDefault(k => k.Id == model.Id);

                    if (existingKnowledge == null)
                    {
                        return HttpNotFound();
                    }

                    // 更新基本欄位
                    existingKnowledge.Title = model.Title;
                    existingKnowledge.Description = model.Description;
                    existingKnowledge.IsTop = model.IsTop;
                    existingKnowledge.AdminId = model.AdminId;

                    // 處理檔案上傳
                    if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
                    {
                        var uploadsFolder = Server.MapPath("~/Uploads/Knowledge");
                        if (!System.IO.Directory.Exists(uploadsFolder))
                            System.IO.Directory.CreateDirectory(uploadsFolder);

                        // 刪掉舊檔案
                        if (!string.IsNullOrEmpty(existingKnowledge.FilePath))
                        {
                            var oldFile = Server.MapPath(existingKnowledge.FilePath);
                            if (System.IO.File.Exists(oldFile))
                                System.IO.File.Delete(oldFile);
                        }

                        // 儲存新檔案
                        var fileName = System.IO.Path.GetFileName(model.FileUpload.FileName);
                        var path = System.IO.Path.Combine(uploadsFolder, fileName);
                        model.FileUpload.SaveAs(path);
                        existingKnowledge.FilePath = "/Uploads/Knowledge/" + fileName;
                    }

                    // 更新修改時間
                    existingKnowledge.UpdatedDate = DateTime.Now;

                    db.Entry(existingKnowledge).State = EntityState.Modified;
                    db.SaveChanges();

                    // 設定成功訊息與跳轉網址
                    TempData["SuccessMessage"] = "編輯成功！";
                    ViewBag.RedirectUrl = Url.Action("Index");
                    return View(model);
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
                
            ViewBag.AdminId = new SelectList(db.Admins, "Id", "Account", model.AdminId);
            return View(model);
        }

        //POST: Back/Knowledges/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null) 
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            var knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return Json(new { success = false, message = "找不到資料" });
            }

            // 刪除實體檔案
            if (!string.IsNullOrEmpty(knowledge.FilePath))
            {
                var filePath = Server.MapPath(knowledge.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.Knowledges.Remove(knowledge);
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
