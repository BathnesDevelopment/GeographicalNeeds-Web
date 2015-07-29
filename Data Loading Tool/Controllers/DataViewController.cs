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

        public ActionResult CreateViewFirstStep()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            CreateViewModel model = dataAccess.getCreateViewModel();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateViewFirstStep(CreateViewModel model)
        {
            CreateViewCompleteModel fullModel = new CreateViewCompleteModel();

            fullModel.ViewName = model.ViewName;

            TempData["CreateViewModel"] = model;
            TempData["CreateViewCompleteModel"] = fullModel;

            return RedirectToAction("CreateViewSecondStep");
        }

        public ActionResult CreateViewSecondStep()
        {
            DataViewDataAccess dataAccess = new DataViewDataAccess();

            CreateViewModel model = (CreateViewModel)TempData["CreateViewModel"];

            List<CreateViewMeasureModel> models = dataAccess.getViewMeasureModels(model.SelectedMeasureIDs).ToList();

            return View(models);
        }

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
            

            return View(models);
        }

        [HttpPost]
        public ActionResult CreateViewThirdStep(List<CreateViewDimensionModel> models)
        {
            CreateViewCompleteModel fullModel = (CreateViewCompleteModel)TempData["CreateViewCompleteModel"];

            DataViewDataAccess dataAccess = new DataViewDataAccess();

            foreach (CreateViewMeasureDimensionModel item in fullModel.Measures)
            {                
                item.DimValues = dataAccess.convertDimensionViewModelsToDBModels(models, item.MeasureID);
            }

            dataAccess.createBespokeView(fullModel);

            return RedirectToAction("Index");
        }
    }
}
