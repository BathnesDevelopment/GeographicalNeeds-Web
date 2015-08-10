using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// This model is for the View used to specify the details 
    /// for the creation of custom views.
    /// </summary>
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

    /// <summary>
    /// Model used to create a template for a custom view. This is structured around the way in which
    /// the template will be generated and features a hierarchy of 3 model classes.
    /// </summary>
    public class CreateViewCompleteModel
    {
        public String ViewName { get; set; }

        public IEnumerable<CreateViewMeasureDimensionModel> Measures { get; set; }
    }

    public class CreateViewMeasureDimensionModel
    {
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