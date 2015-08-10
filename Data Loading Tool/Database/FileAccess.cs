using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Database
{
    /// <summary>
    /// Class to control writing files to disk and other file based operations. 
    /// </summary>
    public class FileAccess
    {
        /// <summary>
        /// Method to persist a file to a location on disk 
        /// </summary>
        /// <param name="model">The model object that contains the file to be saved</param>
        /// <param name="path">The path that the file should be uploaded to</param>
        public void writeCSVtoDisk(UploadStagingModel model, String path)
        {
            try
            {
                model.attachment.SaveAs(path);
            }
            catch (Exception ex)
            {

            }
        }
    }
}