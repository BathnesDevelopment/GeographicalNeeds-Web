using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    public class GeneralStagingModel
    {
        public int TableID { get; set; }

        [DisplayName("Staging Table Name")]
        public String TableName { get; set; }
    }
}