using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace Data_Loading_Tool.Models
{
    public class StagingDetailModel
    {
        [DisplayName("Staging Dataset Name")]
        public String StagingDatasetName { get; set; }

        public IEnumerable<String> Columns { get; set; }

    }
}