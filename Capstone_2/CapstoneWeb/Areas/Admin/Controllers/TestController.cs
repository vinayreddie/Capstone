using Capstone.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneWeb.Areas.Admin.Controllers
{
    [SessionTimeout]
    public class TestController : Controller
    {
        // GET: Admin/Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAdminDetails()
        {
            return View("Index");
        }
    }
}