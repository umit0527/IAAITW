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
    public class AboutController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/About/About
        public ActionResult About()
        {
            var about = db.Abouts.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(about);
        }

        // GET: Front/About/Organization
        public ActionResult Organization()
        {
            var organization = db.Organizationes.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(organization);
        }

        // GET: Front/About/History
        public ActionResult History()
        {
            var history = db.Histories.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(history);
        }

        // GET: Front/About/Member
        public ActionResult Member()
        {
            return View();
        }

        // GET: Front/About/Expert
        public ActionResult Expert()
        {
            var expert = db.Experts.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(expert);
        }
    }
}
