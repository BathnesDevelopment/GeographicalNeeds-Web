using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A Model representing the data needed to create a Dimension
    /// </summary>
    public class CreateDimensionModel : BaseModel
    {
        public int StagingDatasetID { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Dimension Name")]
        public String DimensionName { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForDimension { get; set; }

        [Required]
        [DisplayName("Column Name in Staging Table")]
        public int DimColumnInStaging { get; set; }
    }
}