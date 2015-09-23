using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Loading_Tool.Models
{
    public class StagingIndexModel : BaseModel
    {
        public IEnumerable<GeneralStagingModel> StagingModels { get; set; }
    }

    /// <summary>
    /// A model which represents the basic information around
    /// a staging table. This can be used in the index screen and in the 
    /// create Screen.
    /// </summary>
    public class GeneralStagingModel : BaseModel
    {
        public int TableID { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        [DisplayName("Staging Table Name")]
        public String TableName { get; set; }
    }
}