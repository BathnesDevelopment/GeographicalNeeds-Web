using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class UploadStagingModel
    {
        public int StagingTableID { get; set; }

        public String UniqueUploadRef { get; set; }

        public Boolean FirstUpload { get; set; }
        
        public Boolean UnpivotData { get; set; }

        public HttpPostedFileBase attachment { get; set; }
    }
}