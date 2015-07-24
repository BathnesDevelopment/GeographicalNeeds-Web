using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    public class MeasureLoadingModel
    {
        public List<DimensionModel> Dimensions { get; set; }
        public List<StagingDimensionModel> StagingDimensions { get; set; }
    }

    public class StagingDimensionModel
    {
        [DisplayName("Column Name in Staging")]
        public String StagingColumnName { get; set; }

        [DisplayName("Value in Staging")]
        public String StagingColumnValue { get; set; }
    }

    public class DimensionModel
    {
        public int DimensionID { get; set; }

        [DisplayName("Dimension Value")]
        public String DimensionValue { get; set; }
    }
}