using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Templates
{
    public partial class InsertMeasureTemplate
    {
        public String FactName { get; set; }

        public IEnumerable<int> DimensionIDs { get; set; }
    }
}