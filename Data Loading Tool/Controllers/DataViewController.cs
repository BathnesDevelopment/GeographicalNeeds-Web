using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

using Data_Loading_Tool.Models;
using Data_Loading_Tool.Database;

namespace Data_Loading_Tool.Controllers
{
    public class DataViewController : Controller
    {
        /// <summary>
        /// The controller method to view a list of all 
        /// the Data Views created within the system. 
        /// This takes no parameters
        /// 
        /// Accessed via /DataView/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            DataViewIndexModel model = new DataViewIndexModel();

            model.DataViewModels = dataAccess.getViews();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = true });
            
            model.Breadcrumbs = trail;

            return View(model);
        }

        /// <summary>
        /// A controller method to display a page which allows the user
        /// to view the data returned from a call to a View in the 
        /// Database. This takes the ID of the View as a parameter. 
        /// There is no postback from this page.
        /// 
        /// Accessed via /DataView/ViewDetails
        /// </summary>
        /// <param name="viewID"></param>
        /// <returns></returns>
        public ActionResult ViewDetails(int viewID)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            DataViewDetailModel model = dataAccess.getViewData(viewID);

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Details", Action = "", Controller = "", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }
      
        /// <summary>
        /// A controller method to navigate to the page where users can create
        /// a bespoke View. This populates the initial models for the view
        /// </summary>
        /// <returns>The View to be displayed</returns>
        public ActionResult CreateView()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            CreateViewModel model = dataAccess.getDefaultCreateViewModel();

            List<ViewColumnModel> columns = new List<ViewColumnModel>();

            model.Columns = columns;

            model.NewColumnModel = dataAccess.getDefaultColumnModel();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }

        /// <summary>
        ///     Postback method that handles the creation of the Data View. If the model is valid
        ///     then it creates the view and redirects back to the Index page. If it is not then
        ///     the Create View screen is displayed with an appropriate message to the user.
        /// </summary>
        /// <param name="model">A completed model</param>
        /// <returns>The view to display</returns>
        [HttpPost]
        public ActionResult CreateView(CreateViewModel model)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            if (!dataAccess.isCreateViewModelValid(model))
            {
                ModelState.AddModelError("ViewName", "The View Name must be unique");

                CreateViewModel tempModel = dataAccess.getDefaultCreateViewModel();

                model.GeographyTypes = tempModel.GeographyTypes;

                model.NewColumnModel = dataAccess.getDefaultColumnModel();

                List<Breadcrumb> trail = new List<Breadcrumb>();

                trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

                model.Breadcrumbs = trail;
                return View(model);
            }

            try
            {
                dataAccess.CreateView(model);
            }
            catch (SqlException)
            {
                ModelState.AddModelError("", "An error occured when creating the Data View. Please ensure that the Data View Name is unique and that the columns are uniquely named within it.");

                CreateViewModel tempModel = dataAccess.getDefaultCreateViewModel();

                model.GeographyTypes = tempModel.GeographyTypes;

                model.NewColumnModel = dataAccess.getDefaultColumnModel();

                List<Breadcrumb> trail = new List<Breadcrumb>();

                trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

                model.Breadcrumbs = trail;

                return View(model);
            }

            TempData["SuccessMessage"] = String.Format("The Data View - {0}, was successfully created", model.ViewName);            

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     A helper method which returns the partial view of the recently created column data
        /// </summary>
        /// <param name="measureID">The ID of the measure to be used</param>
        /// <param name="measureBreakdownID">The ID of the measure Breakdown to be used</param>
        /// <param name="dimValueID">The ID of the Dim Combination to be used</param>
        /// <param name="newColumnName">The name of the new column to be added</param>
        /// <returns>The Partial View that can will be added to the main view</returns>
        public PartialViewResult AddColumnRow(int measureID, int measureBreakdownID, int dimValueID, String newColumnName)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            ViewColumnModel model = dataAccess.getColumnModel(measureID, measureBreakdownID, dimValueID, newColumnName);
            
            return PartialView("ViewColumnControl", model);
        }

        /// <summary>
        ///     Helper method which is used to update the drop down
        ///     of Measure Breakdowns based on a selected
        ///     Measure
        /// </summary>
        /// <param name="measureID">The ID of the selected Measure</param>
        /// <returns>The data to populate the drop down of Measure Breakdowns as JSON</returns>
        public JsonResult GetMeasureBreakdownsForMeasure(int measureID)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            return Json(dataAccess.getMeasureBreakdownsForMeasure(measureID));
        }

        /// <summary>
        ///     Helper method which is used to update the drop down
        ///     of Dimension Set Combinations based on a selected
        ///     Measure Breakdown
        /// </summary>
        /// <param name="measureBreakdownID">The ID of the selected Measure Breakdown</param>
        /// <returns>The data to populate the drop down of Dimension Set Combinations as JSON</returns>
        public JsonResult GetDimensionSetCombinationsForMeasureBreakdown(int measureBreakdownID)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            return Json(dataAccess.getDimensionSetCombinationsForMeasureBreakdowns(measureBreakdownID));
        }
    }
}
