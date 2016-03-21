using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class MeasureBreakdownIndexModel : BaseModel
    {
        public int MeasureID { get; set; }
        public IEnumerable<MeasureBreakdownModel> MeasureBreakdowns {get; set;}
    }

    public class MeasureBreakdownModel : BaseModel
    {
        public int MeasureBreakdownID { get; set; }
        public String MeasureBreakdownName { get; set; }
    }
}