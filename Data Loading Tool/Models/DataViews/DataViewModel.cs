using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    public class DataViewIndexModel : BaseModel
    {
        public IEnumerable<DataViewModel> DataViewModels { get; set; }
    }

    /// <summary>
    /// A model representing a basic high level view of the 
    /// Data Views. 
    /// </summary>
    public class DataViewModel
    {
        public int DataViewID { get; set; }

        [DisplayName("Name of the View")]
        public String DataViewName { get; set; }
    }
}