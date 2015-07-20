using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Database
{
    public class FileAccess
    {

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