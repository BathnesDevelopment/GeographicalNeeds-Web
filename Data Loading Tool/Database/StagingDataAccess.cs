using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Database
{
    /// <summary>
    /// Class which handles all data access relating to staging tables. 
    /// This includes creation, upload of data and modification.
    /// </summary>
    public class StagingDataAccess
    {
        /// <summary>
        /// This method returns a list of all the Staging Tables held in the Database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GeneralStagingModel> getStagingTables()
        {            
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            IEnumerable<GeneralStagingModel> tables = context.StagingDatasets.Select(x => new GeneralStagingModel() { TableName = x.DatasetName, TableID = x.StagingDatasetID });            

            return tables;
        }

        /// <summary>
        /// Validate that the Create Staging Table Model is valid prior to
        /// submission to the database
        /// </summary>
        /// <param name="model">The model to be validated</param>
        /// <returns>Boolean indicating valid or not</returns>
        public Boolean isCreateStagingModelValid(GeneralStagingModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            int count = context.StagingDatasets.Where(x => x.DatasetName.Equals(model.TableName)).Count();

            return count == 0;
        }

        /// <summary>
        /// Method to create a Staging Table initially. This intial creation creates
        /// a record of the table in the meta data tables and then a trigger creates the 
        /// actual table. Initially there are no columns associated with the table
        /// as these are added later when the data is uploaded.
        /// </summary>
        /// <param name="model">
        /// The populated model containing the name of the table to be created.
        /// </param>
        public void createStagingTable(GeneralStagingModel model) 
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            StagingDataset staging = context.StagingDatasets.Create();

            staging.DatasetName = model.TableName;

            context.StagingDatasets.Add(staging);

            context.SaveChanges();

            context.Dispose();
        }

        /// <summary>
        /// Method to return the details of a Staging Table from the 
        /// database. This includes the name of the Staging Table and all
        /// the columns associated with it.
        /// </summary>
        /// <param name="datasetID">The ID of the Staging Table to be returned</param>
        /// <returns></returns>
        public StagingDetailModel getStagingDetails(int datasetID)
        {
            DataTable dt;
            SqlDataAdapter sda;

            StagingDetailModel model = new StagingDetailModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingDatasetName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).DatasetName;

            List<String> columns = context.StagingColumns.Where(x => x.StagingDatasetID.Equals(datasetID)).Select(x => x.ColumnName).ToList();

            String sql = String.Format("Select {1} from [{0}]", model.StagingDatasetName, String.Join(",", columns.Select(x => String.Format("[{0}]", x))));

            using (SqlConnection connection = new SqlConnection(
               context.Database.Connection.ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();

                sda = new SqlDataAdapter(command);
                dt = new DataTable("Results");
                sda.Fill(dt);
            }

            context.Dispose();

            model.Data = dt;

            return model;
        }

        /// <summary>
        /// Main method to insert data into the Staging table from a CSV file. This method is able to create 
        /// the  columns in a table as well if necessary. Also data can be unpivotted if it is required.
        /// </summary>
        /// <param name="fileName">The location of the file on disk</param>
        /// <param name="tableID">The ID of the staging table to be uploaded to</param>
        /// <param name="uniqueRef">The unique reference associated with this upload</param>
        /// <param name="unpivot">A boolean flag indicating whether or not the data needs to be unpivoted before loading</param>
        /// <param name="firstLoad">A boolean flag indicating whether or not this is the first upload of data to the table. If it is then the columns are created in the staging table</param>
        /// <param name="geographyColumn">The column in the CSV file which contains the Geography reference. This is used in unpivoting and is not needed otherwise</param>
        public void updateTableFromCSV(String fileName, int tableID, String uniqueRef, bool unpivot, bool firstLoad, String geographyColumn)
        {
            DataTable dt = readCSVtoDataTable(fileName, uniqueRef);

            if (unpivot)
            {
                dt = unPivotData(dt, geographyColumn);                
            }

            if (firstLoad)
            {
                addColumnsToTables(dt, tableID);
            }

            copyDataToTable(dt, tableID);
        }

        /// <summary>
        /// Method to extract the CSV data and return it in the form of a DataTable. This Table is a 
        /// copy of the data with the addition of a column for the Upload Reference
        /// </summary>
        /// <param name="fileName">The location of the CSV file on disk</param>
        /// <param name="uniqueUploadRef">The unique upload reference passed from the user that is attached to this upload</param>
        /// <returns></returns>
        private DataTable readCSVtoDataTable(String fileName, String uniqueUploadRef)
        {
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            dt.Columns.Add(new DataColumn("UploadRef", typeof(String)));

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn(s)));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                List<String> rowValues = new List<string>();
                rowValues.Add(uniqueUploadRef);
                rowValues.AddRange(r.Split(line));

                row.ItemArray = rowValues.ToArray();

                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            return dt;
        }

        /// <summary>
        /// A method to unpivot the data contained in a DataTable. This works for single dimensioned data
        /// and requires the specification of a the Geography column to use.
        /// </summary>
        /// <param name="table">The table to unpivot</param>
        /// <param name="geographyColumn">The column to use as the Geography</param>
        /// <returns></returns>
        private DataTable unPivotData(DataTable table, String geographyColumn)
        {
            DataTable retVal = new DataTable();

            retVal.Columns.Add("UploadRef");
            retVal.Columns.Add("Geography");
            retVal.Columns.Add("DimValue");
            retVal.Columns.Add("Count");

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (column.ColumnName != "UploadRef" && column.ColumnName != geographyColumn)
                    {
                        DataRow newRow = retVal.NewRow();

                        newRow["UploadRef"] = row["UploadRef"];
                        newRow["Geography"] = row[geographyColumn];
                        newRow["DimValue"] = column.ColumnName;
                        newRow["Count"] = row[column.ColumnName];

                        retVal.Rows.Add(newRow);
                    }
                }

            }

            return retVal;
        }

        /// <summary>
        /// A method to add Columns to a Staging Table. This adds them to both the real table 
        /// and the meta data about the table. The columns to add are based from the Data
        /// Table provided.
        /// </summary>
        /// <param name="dt">The Data Table to use to get the Columns to be added</param>
        /// <param name="tableID">The ID of the Staging Table to be added to.</param>
        private void addColumnsToTables(DataTable dt, int tableID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            string datasetName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(tableID)).DatasetName;

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "UploadRef")
                {
                    SqlParameter param1 = new SqlParameter("@tableName", datasetName);
                    SqlParameter param2 = new SqlParameter("@columnName", column.ColumnName);
                    context.Database.ExecuteSqlCommand("AddColumnToTable @tableName, @columnName",
                                                      param1, param2);
                }

                StagingColumn newColumn = new StagingColumn();
                newColumn.ColumnName = column.ColumnName;
                newColumn.StagingDatasetID = tableID;

                context.StagingColumns.Add(newColumn);
            }

            context.SaveChanges();

            context.Dispose();
        }

        /// <summary>
        /// Method to add the data from a DataTable to a Staging Table. This 
        /// uses a SQLBulkCopy to update the database
        /// </summary>
        /// <param name="dt">The Table containing the data</param>
        /// <param name="tableID">The ID of the table to be updated.</param>
        private void copyDataToTable(DataTable dt, int tableID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            string datasetName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(tableID)).DatasetName;

            using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                //make our command and dispose at the end
                using (var copy = new SqlBulkCopy(conn))
                {

                    //Open our connection
                    conn.Open();

                    ///Set target table and tell the number of rows
                    copy.DestinationTableName = datasetName;
                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        //Send it to the server
                        copy.WriteToServer(dt);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            context.Dispose();       
        }

    }
}