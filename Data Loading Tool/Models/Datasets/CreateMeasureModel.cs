using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    public class CreateMeasureModel
    {
        public String StagingTableName { get; set; }
        public String MeasureName { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForMeasure { get; set; }
        public int? MeasureColumnID { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForGeography { get; set; }
        public int GeographyColumnID { get; set; }

        public IList<CreateMeasureDetailModel> MeasureDetails { get; set; }
    }

    public class CreateMeasureDetailModel
    {
        public String StagingColumnName { get; set; }
        public int StagingColumnID { get; set; }

        public IEnumerable<SelectListItem> AvailableDimensions { get; set; }
        public int? DimensionValueID { get; set; }
    }
}