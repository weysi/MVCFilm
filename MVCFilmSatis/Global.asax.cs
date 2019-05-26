using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCFilmSatis
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //İstek gelince yapılacaklar
        protected void Application_BeginRequest()
        {
            var lang = Request.Cookies["lang"];

            if(lang != null)
            {
                string language = lang.Value;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
