using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Templates
{
    public partial class InsertMeasureValuesTemplate
    {
        public Boolean UseMeasureColumn { get; set; }

        public String MeasureColumnName { get; set; }

        public MeasureLoadingModel Details { get; set; }
        
        public String MeasureName { get; set; }
        
        public String StagingGeographyColumn { get; set; }

        public String StagingTableName { get; set; }
    }
}