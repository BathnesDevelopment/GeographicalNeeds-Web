using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Templates
{
    public partial class CreateDefaultViewTemplate
    {
        public IEnumerable<String> DimensionNames { get; set; }

        public String FactName { get; set; }
    }
}