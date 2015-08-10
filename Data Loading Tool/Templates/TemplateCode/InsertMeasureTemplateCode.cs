using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Templates
{
    /// <summary>
    /// A class used to attach properties to the 
    /// Insert Measure Template file
    /// </summary>
    public partial class InsertMeasureTemplate
    {
        public String FactName { get; set; }

        public IEnumerable<int> DimensionIDs { get; set; }
    }
}