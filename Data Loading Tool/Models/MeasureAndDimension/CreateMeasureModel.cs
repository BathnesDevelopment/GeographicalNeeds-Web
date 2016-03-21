using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    public class CreateMeasureModel_old : BaseModel
    {
        [Required]
        [StringLength(1000)]
        [DisplayName("Measure Name")]
        public String MeasureName { get; set; }

        public IEnumerable<SelectListItem> DimensionsForMeasureBreakdown { get; set; }

        public List<int> selectedDimensions { get; set; }
    }
}