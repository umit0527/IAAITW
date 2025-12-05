using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MvcPaging;

namespace IAAITW.Areas.Front.Controllers
{
    public class NewsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/News
        public ActionResult Index(int? page)
        {
            //一頁幾筆資料
            var pageSize = 5;

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
            var news = db.News.Include(n => n.Updater).Include(n => n.Publisher);
            var result = news.OrderByDescending(x => x.IsPinned)
                             .ThenByDescending(x => x.UpdatedDate).ToPagedList(page.Value, pageSize);
            
            return View(result);
        }

        // GET: Front/News/Details/5
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

        // GET: Front/News/Create
        public ActionResult Create()
        {
            ViewBag.UpdaterId = new SelectList(db.Admins, "Id", "Account");
            ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account");
            return View();
        }

        // POST: Front/News/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UpdaterId = new SelectList(db.Admins, "Id", "Account", news.UpdaterId);
            ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account", news.PublisherId);
            return View(news);
        }

        // GET: Front/News/Edit/5
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
            ViewBag.UpdaterId = new SelectList(db.Admins, "Id", "Account", news.UpdaterId);
            ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account", news.PublisherId);
            return View(news);
        }

        // POST: Front/News/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,CoverImage,CreatedDate,UpdatedDate,PublisherId,UpdaterId,IsPinned")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UpdaterId = new SelectList(db.Admins, "Id", "Account", news.UpdaterId);
            ViewBag.PublisherId = new SelectList(db.Admins, "Id", "Account", news.PublisherId);
            return View(news);
        }

        // GET: Front/News/Delete/5
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

        // POST: Front/News/Delete/5
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
