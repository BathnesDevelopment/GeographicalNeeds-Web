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

        public IEnumerable<SelectListItem> Measures { get; set; }

        public IEnumerable<int> SelectedMeasureIDs { get; set; }
    }

    public class CreateViewMeasureModel 
    {
        public String ViewName { get; set; }

        public String MeasureName { get; set; }

        public IEnumerable<CreateViewDimensionModel> DimensionModels { get; set; }

        public IEnumerable<SelectListItem> Dimensions { get; set; }

        public IEnumerable<int> SelectedDimensionIDs { get; set; }

    }

    public class CreateViewDimensionModel
    {
        public String ViewName { get; set; }

        public String MeasureName { get; set; }

        public String DimensionName { get; set; }
        
        public IEnumerable<SelectListItem> DimensionValues { get; set; }

        public IEnumerable<int> SelectedDimensionValueIDs { get; set; }
    }

    public class CreateViewCompleteModel
    {
        public String ViewName { get; set; }

        public IEnumerable<CreateViewMeasureDimensionModel> Measures { get; set; }
    }

    public class CreateViewMeasureDimensionModel
    {
        private IEnumerable<String> _dimValues;

        public int MeasureID {get; set;}
        public String MeasureName { get; set; }
        public IEnumerable<String> DimensionValues { get{return Dimensions.SelectMany(x => x.DimensionValues);} }
        public IEnumerable<CreateViewDimValueModel> Dimensions {get; set;}
    }

    public class CreateViewDimValueModel
    {
        public int DimensionID {get; set;}
        public IEnumerable<int>  DimensionValueIDs{ get; set;}
        public IEnumerable<String> DimensionValues { get; set; }
    }
   
}