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
        /// <param name="model">The completed model to be passed into the data access classes.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDimension(CreateDimensionModel model)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createDimension(model);

            return RedirectToAction("Index", "Staging");
        }

        /// <summary>
        /// The controller method to navigate to the page used to 
        /// initiate the creation of a Measure. This is the first page in a 
        /// two page process. Initially all that is needed is a Staging
        /// Dataset ID to use to populate the model.
        /// 
        /// Accessed from /Dataset/CreateMeasure
        /// </summary>
        /// <param name="stagingDatasetID"></param>
        /// <returns></returns>
        public ActionResult CreateMeasure(int stagingDatasetID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            return View(dataAccess.populateCreateMeasureModel(stagingDatasetID));
        }


        /// <summary>
        /// The postback method from the first phase of the Create
        /// Measure process. This makes calls to the Data Access
        /// layer and then builds up the model to pass to the next phase. 
        /// As part of this process the data is held within TempData.
        /// </summary>
        /// <param name="model">The populated CreateMeasureModel from the page</param>
        /// <returns></returns>
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

        /// <summary>
        /// The controller method for the second phase in the process
        /// to create a Measure. This completes the population of the 
        /// appropriate model. 
        /// 
        /// This is accessed from /Dataset/AddMeasureValues although this 
        /// will not be reached directly but as a redirect from the first 
        /// phase of the process.
        /// </summary>
        /// <param name="stagingTableName">The name of the Staging Table that the data is sourced from</param>
        /// <param name="measureName">The name of the Measure to be created</param>
        /// <param name="measureColumnStagingID">The Column in the Staging table that contains the measure values</param>
        /// <param name="geographyColumnID">The Column in the Staging table that contains the Geography</param>
        /// <returns></returns>
        public ActionResult AddMeasureValues(String stagingTableName, String measureName, int? measureColumnStagingID, int geographyColumnID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            IEnumerable<Tuple<int, int>> mappings = (IEnumerable<Tuple<int, int>>)TempData["DimensionMappings"];

            MeasureValueModel model = dataAccess.populateMeasureValueModel(mappings, stagingTableName, measureName, measureColumnStagingID, geographyColumnID);
            
            return View(model);
        }

        /// <summary>
        /// The controller method that handles postback from the Add Measure Values
        /// page. This takes a completed model and passes it to the Data Access classes
        /// to complete the process to create a Measure.
        /// </summary>
        /// <param name="model">A completed model from the View.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMeasureValues(MeasureValueModel model)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            dataAccess.createMeasureValues(model);                           

            return RedirectToAction("Index", "Staging");
        }       
    }
}
