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
    /// A model that represents the data needed to create a Measure.
    /// This contains a list of specific dimension to dimension mappings which
    /// is contained in the subset of CreateMeasureDetailModels 
    /// </summary>
    public class CreateMeasureModel : BaseModel, IValidatableObject
    {
        public int StagingDatasetID { get; set; }

        [DisplayName("Staging Table Name")]
        public String StagingTableName { get; set; }
        
        [Required]
        [StringLength(100)]
        [DisplayName("Measure Name")]
        public String MeasureName { get; set; }

        public IEnumerable<SelectListItem> StagingColumnsForMeasure { get; set; }
                
        [DisplayName("Measure Column in Staging")]
        public int? MeasureColumnID { get; set; }
       
        public IEnumerable<SelectListItem> StagingColumnsForGeography { get; set; }

        [Required]
        [DisplayName("Geography Column in Staging")]
        public int GeographyColumnID { get; set; }

        public IEnumerable<SelectListItem> GeographyTypes { get; set; }
        
        [Required]
        [DisplayName("Geography Type")]
        public int GeographyTypeID { get; set; }

        public IList<CreateMeasureDetailModel> MeasureDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int numberOfDimsSpecified = MeasureDetails.Where(x => x.DimensionValueID.HasValue).Count();
            if (numberOfDimsSpecified == 0)
            {
                yield return new ValidationResult("At least one Dimension must be specified");
            }
        }
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