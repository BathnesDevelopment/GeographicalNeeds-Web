using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Controller which returns the home index page. 
        /// No data calls or parameters are needed for this
        /// 
        /// Accessed via /Home/index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
