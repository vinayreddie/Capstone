﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Capstone
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional },
                namespaces: new[] { "Capstone.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Areas",
            //    url: "{area}/{controller}/{action}/{id}",
            //    defaults: new {Area="User", controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "Capstone.Controllers" }
            //);
        }
    }
}
