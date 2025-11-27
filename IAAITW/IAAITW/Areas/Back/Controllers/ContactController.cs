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
    public class ContactController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Contact
        public ActionResult Index(int? page)
        {
            //指令化
            var contact = db.Contacts.AsQueryable();

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
            var result = contact.OrderByDescending(x => x.SentDate)
                                   .ToPagedList(page.Value, pageSize);

            return View(result);
        }

        // GET: Back/Contact/Details/5
        public ActionResult Details(int id)
        {
            var data = db.Contacts.FirstOrDefault();

            if (data == null)
            {
                // 沒資料，給 View 顯示空畫面
                ViewBag.HasData = false;
                return View();
            }

            ViewBag.HasData = true;
            return View(data);

            //var contact = db.Contacts.Find(id);
            //if (contact == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(contact);
        }
    }
}
