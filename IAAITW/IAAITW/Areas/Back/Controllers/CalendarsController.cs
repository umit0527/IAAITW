using IAAITW.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Back.Controllers
{
    [CustomAuthorize(LoginUrl = "~/Back/Admins/Login")]
    public class CalendarsController : Controller
    {
        // GET: Back/Calendar
        public ActionResult Index()
        {
            return View();
        }
    }
}
