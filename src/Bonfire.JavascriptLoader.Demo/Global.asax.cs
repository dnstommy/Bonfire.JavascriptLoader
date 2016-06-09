using System.Web.Mvc;
using System.Web.Routing;

namespace Bonfire.JavascriptLoader.Demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            JavascriptLoaderConfig.Configure();
        }
    }
}
