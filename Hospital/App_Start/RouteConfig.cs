using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hospital
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaultDoctor",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Doctors", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultPatient",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Patients", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
