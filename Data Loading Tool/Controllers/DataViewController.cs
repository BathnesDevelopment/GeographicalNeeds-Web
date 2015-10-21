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
        /// A controller method to access the first page of the Create 
        /// Custom View process. This needs no parameters. 
        /// 
        /// This is accessed via /DataView/CreateViewFirstStep
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateViewFirstStep()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            CreateViewModel model = dataAccess.getCreateViewModel();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

            model.Breadcrumbs = trail;
            
            return View(model);
        }

        /// <summary>
        /// A controller method that handles the postback from the
        /// first page of the Create View process. This stores values
        /// in the TempData store and then redirects to the second phase.
        /// </summary>
        /// <param name="model"> The completed model passed back from the view</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateViewFirstStep(CreateViewModel model)
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            if (dataAccess.isCreateViewModelValid(model))
            {
                CreateViewCompleteModel fullModel = new CreateViewCompleteModel();

                fullModel.ViewName = model.ViewName;

                TempData["CreateViewModel"] = model;
                TempData["CreateViewCompleteModel"] = fullModel;

                return RedirectToAction("CreateViewSecondStep");
            }

            ModelState.AddModelError("ViewName", "The Data View Name must be unique");
            model = dataAccess.getCreateViewModel();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
            
        }

        /// <summary>
        /// A controller method which displays the view for the 
        /// second stage of the process to create a Custom View. 
        /// The data needed to do this is held in the TempData store initially
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateViewSecondStep()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            CreateViewModel model = (CreateViewModel)TempData["CreateViewModel"];

            CreateViewMeasureModelList listModel = new CreateViewMeasureModelList();

            List<CreateViewMeasureModel> models = dataAccess.getViewMeasureModels(model.SelectedMeasureIDs).ToList();

            listModel.Models = models;

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

            listModel.Breadcrumbs = trail;

            return View(listModel);
        }

        /// <summary>
        /// The postback method for the second stage in the process to create
        /// a Custom View. This places the necessary data into the TempData
        /// store for access in the third stage.
        /// </summary>
        /// <param name="models">The data entered at the second stage of the process</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateViewSecondStep(List<CreateViewMeasureModel> models)
        {
            CreateViewCompleteModel fullModel = (CreateViewCompleteModel)TempData["CreateViewCompleteModel"];

            DataViewDataAccess dataAccess = new DataViewDataAccess();

            fullModel.Measures = dataAccess.convertMeasureViewModelsToDBModels(models);

            TempData["CreateViewCompleteModel"] = fullModel;

            TempData["SecondStepModels"] = models;

            return RedirectToAction("CreateViewThirdStep");

        }

        /// <summary>
        /// A controller method which displays the view for the 
        /// third and final stage of the process to create a Custom View. 
        /// The data needed to do this is held in the TempData store initially
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateViewThirdStep()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            IEnumerable<CreateViewMeasureModel> measureIDs = (IEnumerable<CreateViewMeasureModel>)TempData["SecondStepModels"];
            List<CreateViewDimensionModel> models = new List<CreateViewDimensionModel>();

            foreach (CreateViewMeasureModel measure in measureIDs)
            {
                if (measure.SelectedDimensionIDs != null)
                {
                    foreach (int dimID in measure.SelectedDimensionIDs)
                    {
                        CreateViewDimensionModel model = dataAccess.getViewDimensionModels(dimID);
                        model.MeasureName = measure.MeasureName;
                        models.Add(model);
                    }
                }
                
            }

            CreateViewDimensionModelList listModel = new CreateViewDimensionModelList();

            listModel.Models = models;

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Data View Index", Action = "Index", Controller = "DataView", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Data View", Action = "", Controller = "", isCurrent = true });

            listModel.Breadcrumbs = trail;

            return View(listModel);
        }

        /// <summary>
        /// A controller method to handle the postback from the final stage in the process
        /// to create a custom view. This takes back the data from the view and 
        /// passes it to the Data Access classes, before redirecting back to the 
        /// main Data View index page.
        /// </summary>
        /// <param name="models">The data entered into the view</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateViewThirdStep(List<CreateViewDimensionModel> models)
        {
            CreateViewCompleteModel fullModel = (CreateViewCompleteModel)TempData["CreateViewCompleteModel"];

            DataViewDataAccess dataAccess = new DataViewDataAccess();

            foreach (CreateViewMeasureDimensionModel item in fullModel.Measures)
            {                
                item.Dimensions = dataAccess.convertDimensionViewModelsToDBModels(models, item.MeasureID);
            }

            try
            {
                dataAccess.createBespokeView(fullModel);
            }
            catch (SqlException)
            {
                List<CreateViewDimensionModel> newModels = new List<CreateViewDimensionModel>();

                foreach (CreateViewDimensionModel x in models)
                {
                    CreateViewDimensionModel model = dataAccess.getViewDimensionModels(x.DimensionID);
                    model.MeasureName = x.MeasureName;
                    newModels.Add(model);
                }

                ModelState.AddModelError("", "An error occured when creating the view");

                TempData["CreateViewCompleteModel"] = fullModel;


                return View(newModels);                    
            }

            TempData["SuccessMessage"] = String.Format("The Data View - {0}, was successfully created", fullModel.ViewName);
            return RedirectToAction("Index");
        }
    }
}
