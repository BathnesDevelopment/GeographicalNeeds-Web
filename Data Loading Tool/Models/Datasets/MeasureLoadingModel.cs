using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class MeasureLoadingModel
    {
        public List<DimensionModel> Dimensions { get; set; }
        public List<StagingDimensionModel> StagingDimensions { get; set; }
    }

    public class StagingDimensionModel
    {
        public String StagingColumnName { get; set; }

        public String StagingColumnValue { get; set; }
    }

    public class DimensionModel
    {
        public int DimensionID { get; set; }

        public String DimensionValue { get; set; }
    }
}