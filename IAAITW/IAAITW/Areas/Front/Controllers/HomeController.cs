using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;
//using static IAAITW.Models.News;

namespace IAAITW.Areas.Front.Controllers
{
    public class HomeController : Controller
    {
        DBMdoelContext db = new DBMdoelContext();
        // GET: Front/Home
        public ActionResult Index()
        {
            var newsList = db.News.OrderByDescending(n => n.IsPinned)
                                  .ThenByDescending(n => n.UpdatedDate)
                                  .ToList();
            return View(newsList);
        }
    }
}
