using IAAITW.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Back.Controllers
{
    [CustomAuthorize(LoginUrl = "~/Back/Admins/Login")]
    public class LisencesController : Controller
    {
        // GET: Back/Lisences
        public ActionResult Index()
        {
            return View();
        }
    }
}
