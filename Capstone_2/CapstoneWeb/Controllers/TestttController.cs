using Capstone.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace CapstoneWeb.Controllers
{
    public class TestttController : Controller
    {
        // GET: Testtt
        public ActionResult Index()
        {
            UserBAL obj = new UserBAL();
            
             return View();
        }
    }
}