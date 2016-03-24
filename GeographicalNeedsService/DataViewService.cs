using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace GeographicalNeedsService
{    
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]        
    public class DataViewService : IDataViewService
    {
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        public DataTable GetViewData(String viewName)
        {
            return new DataTable();
        }
    }
}
