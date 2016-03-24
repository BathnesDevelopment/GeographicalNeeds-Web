using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.ServiceModel.Activation;

using DataAccessServices.Data_Access;

namespace DataAccessServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class GeographicalNeedsService : IGeographicalNeedsService
    {
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        public DataTable GetViewData(String viewName)
        {
            DataViewsDataAccess dataAccess = new DataViewsDataAccess();

            return dataAccess.getViewData(viewName);
        }
    }
}
