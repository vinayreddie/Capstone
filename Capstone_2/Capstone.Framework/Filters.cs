using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;



namespace Capstone.Framework
{
    public class SessionTimeoutAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
    //[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    //public class CheckSessionOutAttribute : System.Web.Http.Filters.ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        HttpContext context = HttpContext.Current;
    //        if (context.Session != null)
    //        {
    //            if (context.Session.IsNewSession)
    //            {
    //                string sessionCookie = context.Request.Headers["Cookie"];

    //                if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
    //                {
    //                    FormsAuthentication.SignOut();
    //                    string redirectTo = "~/Account/Login";
    //                    if (!string.IsNullOrEmpty(context.Request.RawUrl))
    //                    {
    //                        redirectTo = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
    //                        filterContext.Result = new RedirectResult(redirectTo);
    //                        return;
    //                    }

    //                }
    //            }
    //        }

    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}
