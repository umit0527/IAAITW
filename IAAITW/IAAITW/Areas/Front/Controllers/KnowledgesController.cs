using IAAITW.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IAAITW.Areas.Front.Controllers
{
    public class KnowledgesController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Knowledge
        public ActionResult Index(int? page)
        {
            //指令化
            var knowledges = db.Knowledges.AsQueryable();
            
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
            var result = knowledges.OrderByDescending(x => x.IsTop)
                                   .ThenByDescending(x => x.UpdatedDate)
                                   .ToPagedList(page.Value, pageSize);

            return View(result);
        }

        //檔案下載
        public ActionResult Download(string fileName)
        {
            var path = Server.MapPath(fileName);
            var contentType = MimeMapping.GetMimeMapping(fileName);
            var downloadName = System.IO.Path.GetFileName(path);
            return File(path, contentType, downloadName);
        }

        // GET: Front/Knowledge/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Front/Knowledge/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Front/Knowledge/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Front/Knowledge/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Front/Knowledge/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Front/Knowledge/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Front/Knowledge/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
