using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    public class CreateViewModel
    {
        public String ViewName { get; set; }

        public IEnumerable<ViewMeasureModel> MeasureModels { get; set; }

        public MultiSelectList Measures { get; set; }

        public IEnumerable<int> SelectedMeasureIDs { get; set; }
    }

    public class ViewMeasureModel 
    {
        public String MeasureName { get; set; }

        public IEnumerable<ViewDimensionModel> DimensionModels { get; set; }

        public MultiSelectList Dimensions { get; set; }

        public IEnumerable<int> SelectedDimensionIDs { get; set; }

    }

    public class ViewDimensionModel
    {
        public String DimensionName { get; set; }

        public IEnumerable<String> ValueModels { get; set; }

        public MultiSelectList DimensionValues { get; set; }

        public IEnumerable<int> SelectedDimensionValueIDs { get; set; }
    }
   
}