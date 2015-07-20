using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class StagingDetailModel
    {
        public String StagingDatasetName { get; set; }

        public IEnumerable<String> Columns { get; set; }

    }
}