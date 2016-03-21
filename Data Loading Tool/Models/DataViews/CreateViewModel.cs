using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Loading_Tool.Models
{
    public class CreateViewModel : BaseModel
    {
        [Required]
        [StringLength(100)]
        [DisplayName("View Name")]
        public String ViewName { get; set; }

        public IEnumerable<SelectListItem> GeographyTypes { get; set; }

        [Required]
        [DisplayName("Geography Level of View")]
        public int SelectedGeographyType { get; set; }

        public IEnumerable<ViewColumnModel> Columns { get; set; }
        public CreateViewColumnModel NewColumnModel { get; set; }
    }

    public class CreateViewColumnModel : BaseModel 
    {
        [Required]
        [DisplayName("Column Name")]
        public String ColummName { get; set; }

        public IEnumerable<SelectListItem> Measures { get; set; }

        [Required]
        [DisplayName("Measure")]
        public int SelectedMeasure { get; set; }

        public IEnumerable<SelectListItem> MeasureBreakdowns { get; set; }

        [Required]
        [DisplayName("Measure Breakdown")]
        public int SelectedMeasureBreakdown { get; set; }

        public IEnumerable<SelectListItem> DimensionValues { get; set; }

        [Required]
        [DisplayName("Dimension Value")]
        public int SelectedDimensionValue { get; set; }
    }

    public class ViewColumnModel : BaseModel
    {
        [Required]
        [DisplayName("Column Name")]
        public String ColumnName { get; set; }

        [Required]
        [DisplayName("Measure")]
        public String SelectedMeasure { get; set; }

        public int SelectedMeasureID { get; set; }

        [Required]
        [DisplayName("Measure Breakdown")]
        public String SelectedMeasureBreakdown { get; set; }

        public int SelectedMeasureBreakdownID { get; set; }

        [Required]
        [DisplayName("Dimension Value")]
        public String SelectedDimensionValue { get; set; }

        public int SelectedDimensionValueID { get; set; }
    }
}