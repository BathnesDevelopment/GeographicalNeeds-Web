using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Loading_Tool.Models
{
    public class CreateMeasureBreakdownModel : BaseModel
    {
        public int MeasureID { get; set; }

        public String MeasureName { get; set; }

        public String MeasureBreakdownName { get; set; }

        public IEnumerable<SelectListItem> DimensionsForMeasureBreakdown { get; set; }

        public List<int> selectedDimensions { get; set; }
    }
}