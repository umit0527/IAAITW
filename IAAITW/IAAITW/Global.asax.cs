using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IAAITW
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                HttpException httpException = exception as HttpException;
                int httpCode = httpException?.GetHttpCode() ?? 500;

                // 只處理 404 和 500 錯誤
                if (httpCode == 404 || httpCode == 500)
                {
                    // 清除錯誤狀態
                    Server.ClearError();

                    string requestPath = Request.AppRelativeCurrentExecutionFilePath.ToLower();
                    string redirectUrl = "";

                    // 判斷前台或後台並導向相應的錯誤頁面
                    if (requestPath.StartsWith("~/front/"))
                    {
                        if (httpCode == 404)
                            redirectUrl = "~/Front/Error/Error404";
                        else // 403 和 500 都導向 ServerError
                            redirectUrl = "~/Front/Error/ServerError";
                    }
                    else if (requestPath.StartsWith("~/back/"))
                    {
                        if (httpCode == 404)
                            redirectUrl = "~/Back/Error/error404";
                        else // 403 和 500 都導向 error500
                            redirectUrl = "~/Back/Error/error500";
                    }
                    else
                    {
                        // 預設導向前台錯誤頁面
                        if (httpCode == 404)
                            redirectUrl = "~/Front/Error/Error404";
                        else
                            redirectUrl = "~/Front/Error/ServerError";
                    }

                    // 使用 Response.Redirect 而非 Server.Transfer
                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        Response.Redirect(Request.ApplicationPath.TrimEnd('/') +
                                        VirtualPathUtility.ToAbsolute(redirectUrl), false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }
    }
}
