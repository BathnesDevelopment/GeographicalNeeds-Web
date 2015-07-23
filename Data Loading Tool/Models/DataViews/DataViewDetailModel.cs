using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Data_Loading_Tool.Models
{
    public class DataViewDetailModel
    {
        public String ViewName { get; set; }

        public DataTable ViewData { get; set; }
    }
}