using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A Model representing the data needed to create a Dimension
    /// </summary>
    public class CreateDimensionModel
    {
        [DisplayName("Dimension Name")]
        public String DimensionName { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForDimension { get; set; }
        public int DimColumnInStaging { get; set; }
    }
}