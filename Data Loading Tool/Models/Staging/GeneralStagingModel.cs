using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model which represents the basic information around
    /// a staging table. This can be used in the index screen and in the 
    /// create Screen.
    /// </summary>
    public class GeneralStagingModel
    {
        public int TableID { get; set; }

        [DisplayName("Staging Table Name")]
        public String TableName { get; set; }
    }
}