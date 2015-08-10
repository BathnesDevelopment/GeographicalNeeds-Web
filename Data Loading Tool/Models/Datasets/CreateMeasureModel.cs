using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model that represents the data needed to create a Measure.
    /// This contains a list of specific dimension to dimension mappings which
    /// is contained in the subset of CreateMeasureDetailModels 
    /// </summary>
    public class CreateMeasureModel
    {
        [DisplayName("Staging Table Name")]
        public String StagingTableName { get; set; }
        
        [DisplayName("Measure Name")]
        public String MeasureName { get; set; }

        [DisplayName("Measure Column in Staging")]
        public IEnumerable<SelectListItem> StagingColumnsForMeasure { get; set; }
        [DisplayName("Measure Column in Staging")]
        public int? MeasureColumnID { get; set; }

        [DisplayName("Geography Column in Staging")]
        public IEnumerable<SelectListItem> StagingColumnsForGeography { get; set; }
        [DisplayName("Geography Column in Staging")]
        public int GeographyColumnID { get; set; }

        public IList<CreateMeasureDetailModel> MeasureDetails { get; set; }
    }

    /// <summary>
    /// A model that represents the individual mappings 
    /// between staging columns and the dimensions in the 
    /// database
    /// </summary>
    public class CreateMeasureDetailModel
    {
        [DisplayName("Staging Column")]
        public String StagingColumnName { get; set; }
        public int StagingColumnID { get; set; }

        [DisplayName("Dimension Name")]
        public IEnumerable<SelectListItem> AvailableDimensions { get; set; }
        [DisplayName("Dimension Name")]
        public int? DimensionValueID { get; set; }
    }
}