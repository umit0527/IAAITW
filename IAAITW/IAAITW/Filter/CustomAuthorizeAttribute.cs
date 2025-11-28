using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Filter
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        public string LoginUrl { get; set; } // 可以指定不同登入頁

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 已登入但沒有權限
                filterContext.Result = new HttpStatusCodeResult(403);
            }
            else
            {
                // 未登入 → 導向指定的登入頁
                filterContext.Result = new RedirectResult(LoginUrl);
            }
        }
    }
}