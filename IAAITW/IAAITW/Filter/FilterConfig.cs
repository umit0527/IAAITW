using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Filter
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // 保留原本的 HandleError
            filters.Add(new HandleErrorAttribute());

            // 自訂全域例外導向 Error500
            filters.Add(new HandleErrorAttribute
            {
                ExceptionType = typeof(System.Exception), // 捕捉所有 Exception
                View = "~/Areas/Back/Views/Error/ServerError.cshtml"
            });
        }
    }
}