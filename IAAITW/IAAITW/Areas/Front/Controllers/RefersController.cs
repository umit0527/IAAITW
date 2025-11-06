using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;

namespace IAAITW.Areas.Front.Controllers
{
    public class RefersController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Refers
        public ActionResult Index()
        {
            var refers = db.Refers.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(refers.ToList());
        }
    }
}
