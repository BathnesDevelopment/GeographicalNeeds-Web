using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Data_Loading_Tool.Models;
using Data_Loading_Tool.Database;

namespace Data_Loading_Tool.Controllers
{
    public class DataViewController : Controller
    {
        //
        // GET: /DataView/

        public ActionResult Index()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();
            
            return View(dataAccess.getViews());
        }

        public ActionResult ViewDetails(int viewID)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            DataViewDetailModel model = dataAccess.getViewData(viewID);

            return View(model);
        }

        public ActionResult CreateView()
        {
            return View();
        }
    }
}
