using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    public class MeasureValueModel
    {
        public String StagingTableName { get; set; }
        public String MeasureName { get; set; }
        public String StagingGeographyColumn { get; set; }
        public Boolean UseMeasureColumn { get; set; }
        public String MeasureStagingColumnName { get; set; }
        
        public IList<MeasureValueDetailModel> MeasureValueDetails { get; set; }
    }

    public class MeasureValueDetailModel
    {
        public int DimensionID { get; set; }

        public int DimValueID { get; set; }
        public String DimValue { get; set; }

        public IEnumerable<SelectListItem> StagingDimensionValues { get; set; }
        public String DimValueInStaging { get; set; }

        public String DimColumnInStaging { get; set; }
    }
}