using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GeographicalNeedsService
{
    class DataViewsDataAccess
    {
        public DataTable getViewData(int viewID)
        {
            DataTable dt = new DataTable();

            //SqlDataAdapter sda;

            //Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            //String columns = String.Join(",", context.DataViews.Single(x => x.DataViewID.Equals(viewID)).DataViewColumns.Select(x => String.Format("[{0}]", x.ColumnName)));
            //String viewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            //String sql = String.Format("Select {1} from [{0}]", viewName, columns);

            //context.Dispose();

            //using (SqlConnection connection = new SqlConnection(
            //   context.Database.Connection.ConnectionString))
            //{
            //    SqlCommand command = new SqlCommand(sql, connection);
            //    command.Connection.Open();

            //    sda = new SqlDataAdapter(command);
            //    dt = new DataTable("Results");
            //    sda.Fill(dt);
            //}

            return dt;
        }
    }
}
