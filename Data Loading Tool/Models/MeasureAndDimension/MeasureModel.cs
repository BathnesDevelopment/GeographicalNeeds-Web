using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class MeasureModel
    {
        public int MeasureID { get; set; }

        public String MeasureName { get; set; }
    }

    public class MeasureIndexModel : BaseModel
    {
        public IEnumerable<MeasureModel> Measures { get; set; }
    }
}