using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Front.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Front/Error404
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        // GET: Front/Error500
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }

        // GET: Front/ServerError
        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
