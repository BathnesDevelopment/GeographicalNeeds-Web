using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Templates
{
    /// <summary>
    /// A class used to attach properties to the 
    /// Create Default View Template file
    /// </summary>
    public partial class CreateDefaultViewTemplate
    {
        public IEnumerable<String> DimensionNames { get; set; }

        public String FactName { get; set; }

        public Boolean AggregateQuery { get; set; }
    }
}