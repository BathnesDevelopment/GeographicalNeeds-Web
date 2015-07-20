using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data_Loading_Tool.Database;
using Data_Loading_Tool.Models;


namespace Data_Loading_Tool.Controllers
{
    public class DatasetController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateDimension(int stagingDatasetID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            return View(dataAccess.populateCreateDimensionModel(stagingDatasetID));
        }

        [HttpPost]
        public ActionResult CreateDimension(CreateDimensionModel model)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createDimension(model);

            return RedirectToAction("Index", "Staging");
        }

        public ActionResult CreateMeasure(int stagingDatasetID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            return View(dataAccess.populateCreateMeasureModel(stagingDatasetID));
        }

        [HttpPost]
        public ActionResult CreateMeasure(CreateMeasureModel model)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createMeasure(model);            

            IEnumerable<Tuple<int, int>> dimensionMappings = model.MeasureDetails.Where(x => x.DimensionValueID.HasValue).Select(x => new Tuple<int, int>(x.StagingColumnID, x.DimensionValueID.Value));

            TempData["DimensionMappings"] = dimensionMappings;

            return RedirectToAction("AddMeasureValues", new
            {
                stagingTableName = model.StagingTableName,
                measureName = model.MeasureName,
                measureColumnStagingID = model.MeasureColumnID,
                geographyColumnID = model.GeographyColumnID
            });
        }

        public ActionResult AddMeasureValues(String stagingTableName, String measureName, int? measureColumnStagingID, int geographyColumnID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            IEnumerable<Tuple<int, int>> mappings = (IEnumerable<Tuple<int, int>>)TempData["DimensionMappings"];

            MeasureValueModel model = dataAccess.populateMeasureValueModel(mappings, stagingTableName, measureName, measureColumnStagingID, geographyColumnID);

            
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMeasureValues(MeasureValueModel model)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createMeasureValues(model);                           

            return RedirectToAction("Index", "Staging");
        }

        public ActionResult Temp()
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createMeasureValuesTemp2();
            
            return View();
        }

    }
}
