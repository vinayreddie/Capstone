﻿using Capstone.BAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Capstone
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            UserBAL objBAL = new UserBAL();
            LoginModel model = new LoginModel();
            model.Username = "mounika"; model.Password = "123";
            UserModel user = objBAL.ValidateUser(model);
            Session["User"] = user;
        }

    }
}
