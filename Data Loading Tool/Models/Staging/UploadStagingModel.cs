using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model representing the details captured when uploading data to 
    /// a staging table
    /// </summary>
    public class UploadStagingModel : BaseModel , IValidatableObject
    {
        public int StagingTableID { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Unique Upload Reference")]
        public String UniqueUploadRef { get; set; }

        [DisplayName("Column in Staging that contains a Geography")]
        public String GeographyColumn { get; set; }

        [Required]
        [DisplayName("Is this the first upload?")]
        public Boolean FirstUpload { get; set; }

        [Required]
        [DisplayName("Should the data be unpivoted before loading")]
        public Boolean UnpivotData { get; set; }

        [DataType(DataType.Upload)]
        [Required]
        [DisplayName("File to be uploaded")]
        public HttpPostedFileBase attachment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UnpivotData && string.IsNullOrEmpty(GeographyColumn))
                yield return new ValidationResult("If Unpivoting a Geography Column must be supplied.");
        }
    }
}