using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data_Loading_Tool.Models
{
    public class BaseModel
    {
        public IEnumerable<Breadcrumb> Breadcrumbs { get; set; }
    }

    public class Breadcrumb
    {
        public String LinkText { get; set; }

        public String Controller { get; set; }

        public String Action { get; set; }

        public Boolean isCurrent { get; set; }

    }
}