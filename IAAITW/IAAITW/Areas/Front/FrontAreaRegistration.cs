using System.Web.Mvc;

namespace IAAITW.Areas.Front
{
    public class FrontAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Front";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Front_default",
                "Front/{controller}/{action}/{id}",
                new { area = "Front", controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IAAITW.Areas.Front.Controllers" }
            );
        }
    }
}
