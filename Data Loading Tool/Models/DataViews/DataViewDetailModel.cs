using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;

namespace Data_Loading_Tool.Models
{
    public class DataViewDetailModel
    {
        [DisplayName("Name of the View")]
        public String ViewName { get; set; }

        public DataTable ViewData { get; set; }
    }
}