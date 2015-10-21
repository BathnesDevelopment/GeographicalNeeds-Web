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
    /// A model for the first phase of the process to upload
    /// the data into a Measure. This stores the various high level mappings
    /// and is used for the basis of the template file which will create the 
    /// entries.
    /// </summary>
    public class MeasureValueModel: BaseModel
    {
        [DisplayName("Staging Table Name")]
        public String StagingTableName { get; set; }

        [DisplayName("Measure Name")]
        public String MeasureName { get; set; }

        [DisplayName("Staging Geography Column")]
        public String StagingGeographyColumn { get; set; }

        [DisplayName("Should a Measure Column Be Used")]
        public Boolean UseMeasureColumn { get; set; }

        [DisplayName("Measure Column Name")]
        public String MeasureStagingColumnName { get; set; }

        public int GeographyTypeID { get; set; }
        
        public IList<MeasureValueDetailModel> MeasureValueDetails { get; set; }
    }

    public class MeasureValueDetailModel
    {
        public int DimensionID { get; set; }

        public int DimValueID { get; set; }

        [DisplayName("Dimension Value")]
        public String DimValue { get; set; }

        public IEnumerable<SelectListItem> StagingDimensionValues { get; set; }

        [Required]
        [DisplayName("Value of Dimension in Staging")]
        public String DimValueInStaging { get; set; }

        [DisplayName("Column used in Staging for Dimension")]
        public String DimColumnInStaging { get; set; }
    }
}