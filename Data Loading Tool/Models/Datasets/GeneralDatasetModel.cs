using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    public class GeneralDatasetModel
    {
        public int DatasetID { get; set; }

        public String StagingTableName { get; set; }

        public String MeasureName { get; set; }

        public MultiSelectList StagingColumnsForDimension { get; set; }
        public IEnumerable<int> DimColumnsInStaging { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForMeasure { get; set; }
        public int MeasureColumnInStaging { get; set; }

        public MultiSelectList AvailableDimensions { get; set; }
        public IEnumerable<int> DimIDs { get; set; }        
    }
}