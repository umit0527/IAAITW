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
    public class JobController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Job/Job
        public ActionResult Job()
        {
            var jobs = db.Jobs.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(jobs.ToList());
        }

        // GET: Front/Job/Lisences
        public ActionResult Lisences()
        {
            return View();
        }

        // GET: Front/Job/Refers
        public ActionResult Refer()
        {
            var refers = db.Refers.Include(j => j.Admin).Include(j => j.UpdatedAdmin);
            return View(refers.ToList());
        }

        // GET: Front/Job/Survey
        public ActionResult Survey()
        {
            var surveys = db.Surveys.Include(j => j.Admin).Include(j => j.UpdatedAdmin);

            return View(surveys.ToList());
        }
    }
}
