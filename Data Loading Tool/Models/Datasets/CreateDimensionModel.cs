using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    public class CreateDimensionModel
    {
        public String DimensionName { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForDimension { get; set; }
        public int DimColumnInStaging { get; set; }
    }
}