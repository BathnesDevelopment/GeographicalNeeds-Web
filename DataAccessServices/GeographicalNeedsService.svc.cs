using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using DataAccessServices.Data_Models;
using DataAccessServices.Data_Access;

namespace DataAccessServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class GeographicalNeedsService : IGeographicalNeedsService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "GetViewData")]
        public DataTable GetViewData(int viewID)
        {           
            DataTable dt;
            SqlDataAdapter sda;

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            String columns = String.Join(",", context.DataViews.Single(x => x.DataViewID.Equals(viewID)).DataViewColumns.Select(x => String.Format("[{0}]", x.ColumnName)));
            String viewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            String sql = String.Format("Select {1} from [{0}]", viewName, columns);            

            context.Dispose();

            String connectionString = "data source=.\\SQLEXPRESS;initial catalog=\"Geographical Needs\";integrated security=True;multipleactiveresultsets=True;application name=EntityFramework";

            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                
                sda = new SqlDataAdapter(command);
                dt = new DataTable("Results");
                sda.Fill(dt);
            }

            return dt;
        }
    }
}
