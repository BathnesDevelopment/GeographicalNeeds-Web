using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    /// <summary>
    /// A model which can be used to display all the data
    /// held within a Data View
    /// </summary>
    public class DataViewDetailModel : BaseModel
    {
        [DisplayName("Name of the View")]
        public String ViewName { get; set; }

        public DataTable ViewData { get; set; }
    }
}