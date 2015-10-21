using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            CreateDimensionModel model = dataAccess.populateCreateDimensionModel(stagingDatasetID);

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Dimension", Action = "", Controller = "", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
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

            if (!dataAccess.isCreateDimensionModelValid(model))
            {
                ModelState.AddModelError("DimensionName", "The Dimension Name must be unique");

                model = dataAccess.populateCreateDimensionModel(model.StagingDatasetID);

                List<Breadcrumb> trail = new List<Breadcrumb>();

                trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Create Dimension", Action = "", Controller = "", isCurrent = true });

                model.Breadcrumbs = trail;

                return View(model);            
            }

            try
            {
                dataAccess.createDimension(model);
            }
            catch (SqlException)
            {
                ModelState.AddModelError("", "An error occured when creating the dimension. Please ensure that the Dimension Name is unique and that the values are unique within it.");

                model = dataAccess.populateCreateDimensionModel(model.StagingDatasetID);

                List<Breadcrumb> trail = new List<Breadcrumb>();

                trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
                trail.Add(new Breadcrumb() { LinkText = "Create Dimension", Action = "", Controller = "", isCurrent = true });

                model.Breadcrumbs = trail;

                return View(model);    
            }

            TempData["SuccessMessage"] = String.Format("The Dimension - {0}, was successfully created", model.DimensionName);            
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

            CreateMeasureModel model = dataAccess.populateCreateMeasureModel(stagingDatasetID);

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Measure", Action = "", Controller = "", isCurrent = true });


            model.Breadcrumbs = trail;

            return View(model);
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
            
            if (ModelState.IsValid)
            {
                if (!dataAccess.isCreateMeasureModelValid(model))
                {
                    ModelState.AddModelError("", "An error occured when creating the Measure. Please ensure that the Measure name is unique and that Dimensions have been specified");

                    model = dataAccess.populateCreateMeasureModel(model.StagingDatasetID);

                    List<Breadcrumb> trail = new List<Breadcrumb>();

                    trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
                    trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
                    trail.Add(new Breadcrumb() { LinkText = "Create Measure", Action = "", Controller = "", isCurrent = true });


                    model.Breadcrumbs = trail;
                                    
                    return View(model);
                }
                dataAccess.createMeasure(model);

                IEnumerable<Tuple<int, int>> dimensionMappings = model.MeasureDetails.Where(x => x.DimensionValueID.HasValue).Select(x => new Tuple<int, int>(x.StagingColumnID, x.DimensionValueID.Value));

                TempData["DimensionMappings"] = dimensionMappings;

                return RedirectToAction("AddMeasureValues", new
                {
                    stagingTableName = model.StagingTableName,
                    measureName = model.MeasureName,
                    measureColumnStagingID = model.MeasureColumnID,
                    geographyColumnID = model.GeographyColumnID,
                    geographyTypeID = model.GeographyTypeID
                });
            }

            model = dataAccess.populateCreateMeasureModel(model.StagingDatasetID);
                                    
            return View(model);
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
        public ActionResult AddMeasureValues(String stagingTableName, String measureName, int? measureColumnStagingID, int geographyColumnID, int geographyTypeID)
        {
            DatasetDataAccess dataAccess = new DatasetDataAccess();

            IEnumerable<Tuple<int, int>> mappings = (IEnumerable<Tuple<int, int>>)TempData["DimensionMappings"];

            MeasureValueModel model = dataAccess.populateMeasureValueModel(mappings, stagingTableName, measureName, measureColumnStagingID, geographyColumnID, geographyTypeID);

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Measure", Action = "", Controller = "", isCurrent = true });
            
            model.Breadcrumbs = trail;

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

            TempData["SuccessMessage"] = String.Format("The Measure - {0}, was successfully created", model.MeasureName);
            return RedirectToAction("Index", "Staging");            
        }       
    }
}
