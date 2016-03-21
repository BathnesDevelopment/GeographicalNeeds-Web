using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model representing the details of a Staging Table. This includes
    /// the name of the table and the columns in it.
    /// </summary>
    public class StagingDetailModel : BaseModel
    {
        [DisplayName("Staging Dataset Name")]
        public String StagingDatasetName { get; set; }

        public DataTable Data { get; set; }

    }
}