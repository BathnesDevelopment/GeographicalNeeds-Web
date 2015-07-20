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
    public class StagingDataAccess
    {
        public IEnumerable<GeneralStagingModel> getStagingTables()
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            IEnumerable<GeneralStagingModel> tables = context.StagingDatasets.Select(x => new GeneralStagingModel() { TableName = x.DatasetName, TableID = x.StagingDatasetID });            

            return tables;
        }

        public void createStagingTable(GeneralStagingModel model) 
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            StagingDataset staging = context.StagingDatasets.Create();

            staging.DatasetName = model.TableName;

            context.StagingDatasets.Add(staging);

            context.SaveChanges();

            context.Dispose();
        }

        public StagingDetailModel getStagingDetails(int datasetID)
        {
            StagingDetailModel model = new StagingDetailModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingDatasetName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).DatasetName;

            model.Columns = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => x.ColumnName);

            return model;
        }

        public void updateTableFromCSV(String fileName, int tableID, string uniqueRef, bool unpivot, bool firstLoad)
        {
            DataTable dt = readCSVtoDataTable(fileName, uniqueRef);

            if (unpivot)
            {
                dt = unPivotData(dt);                
            }

            if (firstLoad)
            {
                addColumnsToTables(dt, tableID);
            }

            copyDataToTable(dt, tableID);
        }

        
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

        private DataTable unPivotData(DataTable table)
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
                    if (column.ColumnName != "UploadRef" && column.ColumnName != "super output areas - lower layer")
                    {
                        DataRow newRow = retVal.NewRow();

                        newRow["UploadRef"] = row["UploadRef"];
                        newRow["Geography"] = row["super output areas - lower layer"];
                        newRow["DimValue"] = column.ColumnName;
                        newRow["Count"] = row[column.ColumnName];

                        retVal.Rows.Add(newRow);
                    }
                }

            }


            return retVal;
        }

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