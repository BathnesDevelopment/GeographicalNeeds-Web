using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Data_Loading_Tool.Database;
using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Controllers
{
    public class StagingController : Controller
    {
        /// <summary>
        /// Main index showing the list of staging tables
        /// in the system and providing links to the various 
        /// actions available. No parameter is needed
        /// 
        /// Accessed via /Staging/index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            StagingIndexModel model = new StagingIndexModel();

            model.StagingModels = dataAccess.getStagingTables();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }

        /// <summary>
        /// Controller method that takes the user to the page to
        /// create a staging table. No parameters are needed and
        /// the controller needs no data access. 
        /// 
        /// Accessed via /Staging/CreateStaging
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateStaging()
        {
            GeneralStagingModel model = new GeneralStagingModel();

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Create Staging", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }

        /// <summary>
        /// The postback controller method from the Create Staging
        /// page. This takes a populated GeneralStagingModel and
        /// creates a new table as a result. The page then 
        /// redirects back to the /Staging/Index
        /// </summary>
        /// <param name="model"> A populated GeneralStagingModel ready to be persisted</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateStaging(GeneralStagingModel model)
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            if (!dataAccess.isCreateStagingModelValid(model))
            {
                ModelState.AddModelError("TableName", "The Staging Table Name must be unique");
                return View(model);
            }

            try
            {
                dataAccess.createStagingTable(model);
            }
            catch (SqlException)
            {
                ModelState.AddModelError("", "An error occured when creating the Staging Table. Please ensure that the staging table name is unique and that all columns within it are uniquely named");
                return View(model);                
            }

            TempData["SuccessMessage"] = String.Format("The Staging Table - {0}, was successfully created", model.TableName);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// A controller method to navigate to the page to upload
        /// data into the Staging Table. Take the ID of the staging table as 
        /// a parameter.
        /// 
        /// Accessed via /Staging/UploadToStaging        
        /// </summary>
        /// <param name="tableID">The database ID of the staging table to be uploaded to. </param>
        /// <returns></returns>
        public ActionResult UploadToStaging(int tableID)
        {
            UploadStagingModel model = new UploadStagingModel();
            model.StagingTableID = tableID;

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Upload to Staging", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }

        /// <summary>
        /// The postback method for the UploadToStaging page. This 
        /// takes the completed model and a reference to the file
        /// that contains the data to be uploaded. The method checks 
        /// that there is a file specified then calls the data access 
        /// methods to upload the data.Once completed it
        /// redirects back to the main index page at /Staging/Index
        /// </summary>
        /// <param name="model">A completed UploadStagingModel with the necessary data to write to the database</param>
        /// <param name="FileUpload">A file that contains the raw staging data.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadToStaging(UploadStagingModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.attachment.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.attachment.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/CSVUploads"), fileName);

                    Data_Loading_Tool.Database.FileAccess fileAccess = new Data_Loading_Tool.Database.FileAccess();

                    fileAccess.writeCSVtoDisk(model, path);

                    StagingDataAccess dataAccess = new StagingDataAccess();

                    dataAccess.updateTableFromCSV(path, model.StagingTableID, model.UniqueUploadRef, model.UnpivotData, model.FirstUpload, model.GeographyColumn);
                    
                    TempData["SuccessMessage"] = "The Data was uploaded successfully";
                    return RedirectToAction("Index"); 
                }                           
            }

            return View(model);            
        }

        /// <summary>
        /// A controller method to redirect to a page showing the details relating to 
        /// a staging dataset. This takes the dataset database ID as a parameter and
        /// utilises the data access classes to get the data back
        /// 
        /// This is accessed via /Staging/DatasetDetail
        /// </summary>
        /// <param name="datasetID"></param>
        /// <returns></returns>
        public ActionResult DatasetDetails(int datasetID)
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            StagingDetailModel model = dataAccess.getStagingDetails(datasetID);

            List<Breadcrumb> trail = new List<Breadcrumb>();

            trail.Add(new Breadcrumb() { LinkText = "Home", Action = "Index", Controller = "Home", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Index", Action = "Index", Controller = "Staging", isCurrent = false });
            trail.Add(new Breadcrumb() { LinkText = "Staging Dataset Details", isCurrent = true });

            model.Breadcrumbs = trail;

            return View(model);
        }

    }
}
