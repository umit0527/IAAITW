using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;

namespace IAAITW.Areas.Back.Controllers
{
    public class KnowledgesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Knowledges
        public ActionResult Index()
        {
            var knowledges = db.Knowledges.Include(k => k.Admin);
            return View(knowledges.ToList());
        }

        // GET: Back/Knowledges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
        public ActionResult Create([Bind(Include = "Id,Title,Description,FilePath,FileUpload,UploadUserId,UploadDate")] KnowledgeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var knowledge = new Knowledge
                {
                    Title = model.Title,
                    Description = model.Description,
                    FilePath = model.FilePath,
                    UploadUserId = model.UploadUserId,
                    UploadDate = model.UploadDate
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
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "檔案上傳失敗: " + ex.Message);
                        ViewBag.UploadUserId = new SelectList(db.Admins, "Id", "Account", model.UploadUserId);
                        return View(model);
                    }
                    // 存到資料庫
                    knowledge.FilePath = "/Uploads/Knowledge/" + fileName; // 可存相對路徑
                }
                else
                {
                    ModelState.AddModelError("", "未收到檔案或檔案為空");
                }

                db.Knowledges.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UploadUserId = new SelectList(db.Admins, "Id", "Account", model.UploadUserId);
            return View(model);
        }

        // GET: Back/Knowledges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            ViewBag.UploadUserId = new SelectList(db.Admins, "Id", "Account", knowledge.UploadUserId);
            return View(knowledge);
        }

        // POST: Back/Knowledges/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,FileName,FilePath,UploadUserId,UploadDate")] Knowledge knowledge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UploadUserId = new SelectList(db.Admins, "Id", "Account", knowledge.UploadUserId);
            return View(knowledge);
        }

        // GET: Back/Knowledges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: Back/Knowledges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Knowledge knowledge = db.Knowledges.Find(id);
            db.Knowledges.Remove(knowledge);
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
