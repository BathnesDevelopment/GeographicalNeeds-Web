using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Templates
{
    /// <summary>
    /// A class used to attach properties to the 
    /// Create Custom View Template file
    /// </summary>
    partial class CreateCustomViewTemplate
    {
        public CreateViewCompleteModel model { get; set; }

        public int GeographyAggregationlevel { get; set; }
    }
}