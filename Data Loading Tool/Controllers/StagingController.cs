using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            return View(dataAccess.getStagingTables());
        }

        public ActionResult CreateStaging()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult CreateStaging(GeneralStagingModel model)
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            dataAccess.createStagingTable(model);

            return RedirectToAction("Index");
        }

        public ActionResult UploadToStaging(int tableID)
        {
            UploadStagingModel model = new UploadStagingModel();
            model.StagingTableID = tableID;

            return View(model);
        }

        [HttpPost]
        public ActionResult UploadToStaging(UploadStagingModel model, HttpPostedFileBase FileUpload)
        {
            model.attachment = FileUpload;

            if (model.attachment.ContentLength > 0)
            {
                string fileName = Path.GetFileName(model.attachment.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/CSVUploads"), fileName);

                Data_Loading_Tool.Database.FileAccess fileAccess = new Data_Loading_Tool.Database.FileAccess();

                fileAccess.writeCSVtoDisk(model, path);

                StagingDataAccess dataAccess = new StagingDataAccess();

                dataAccess.updateTableFromCSV(path, model.StagingTableID, model.UniqueUploadRef, model.UnpivotData, model.FirstUpload, model.GeographyColumn);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DatasetDetails(int datasetID)
        {
            StagingDataAccess dataAccess = new StagingDataAccess();

            StagingDetailModel model = dataAccess.getStagingDetails(datasetID);

            return View(model);
        }

    }
}
