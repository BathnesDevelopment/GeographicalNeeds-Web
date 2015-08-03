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
        /// <summary>
        /// Controller method to navigate to the page for
        /// creating a Dimension. This takes the ID of a 
        /// staging dataset which is used to find the values 
        /// for the new dimension.
        /// 
        /// Accessed via /Dataset/CreateDimension
        /// </summary>
        /// <param name="stagingDatasetID"> The Database ID of the staging dataset</param>
        /// <returns></returns>
        public ActionResult CreateDimension(int stagingDatasetID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            return View(dataAccess.populateCreateDimensionModel(stagingDatasetID));
        }

        /// <summary>
        /// The postback method from the Create Dimension page 
        /// which takes a completed model and passes the data to the 
        /// data access classes to create the new Dimension and related
        /// values. Redirects to /Staging/Index when completed.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
    }
}
