﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessServices.Data_Access
{
    public class DataViewsDataAccess
    {
        public DataTable getViewData(String viewName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            String columns = String.Join(",", context.DataViews.Single(x => x.ViewName.Equals(viewName)).DataViewColumns.Select(x => String.Format("[{0}]", x.ColumnName)));            

            String sql = String.Format("Select {1} from [{0}]", viewName, columns);

            context.Dispose();

            using (SqlConnection connection = new SqlConnection(
               context.Database.Connection.ConnectionString))
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