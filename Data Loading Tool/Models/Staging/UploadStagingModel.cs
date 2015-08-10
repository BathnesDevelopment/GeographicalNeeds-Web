using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model representing the details captured when uploading data to 
    /// a staging table
    /// </summary>
    public class UploadStagingModel
    {
        public int StagingTableID { get; set; }

        [DisplayName("Unique Upload Reference")]
        public String UniqueUploadRef { get; set; }

        [DisplayName("Column in Staging that contains a Geography")]
        public String GeographyColumn { get; set; }

        [DisplayName("Is this the first upload?")]
        public Boolean FirstUpload { get; set; }

        [DisplayName("Should the data be unpivoted before loading")]
        public Boolean UnpivotData { get; set; }

        public HttpPostedFileBase attachment { get; set; }
    }
}