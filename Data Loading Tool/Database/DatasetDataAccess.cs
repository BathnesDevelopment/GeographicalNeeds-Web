using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

using Data_Loading_Tool.Models;
using Data_Loading_Tool.Templates;

namespace Data_Loading_Tool.Database
{
    /// <summary>
    /// Class to control all data access to Measures and Dimensions. This includes creation, selection and modification methods.
    /// </summary>
    public class DatasetDataAccess
    {
        /// <summary>
        /// Method to create and populate a CreateMeasureModel ready for the View to use
        /// </summary>
        /// <param name="datasetID">The ID of the Staging Table this Measure is based off</param>
        /// <returns></returns>
        public CreateMeasureModel populateCreateMeasureModel(int datasetID)
        {
            CreateMeasureModel model = new CreateMeasureModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingDatasetID = datasetID;

            model.StagingTableName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).DatasetName;

            model.StagingColumnsForMeasure = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
                                                                                                                                                                    {
                                                                                                                                                                        Text = x.ColumnName,
                                                                                                                                                                        Value = x.StagingColumnID.ToString()
                                                                                                                                                                    });

            model.StagingColumnsForGeography = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
                                                                                                                                                                    {
                                                                                                                                                                        Text = x.ColumnName,
                                                                                                                                                                        Value = x.StagingColumnID.ToString()
                                                                                                                                                                    });

            List<CreateMeasureDetailModel> modelDetails = new List<CreateMeasureDetailModel>();
            
            foreach (StagingColumn column in context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns)
            {
                if (column.ColumnName != "UploadRef")
                {                    
                    CreateMeasureDetailModel detail = new CreateMeasureDetailModel();

                    detail.StagingColumnID = column.StagingColumnID;
                    detail.StagingColumnName = column.ColumnName;

                    detail.AvailableDimensions = context.Dimensions.ToList().Select(x => new SelectListItem() { Text = x.DimensionName, Value = x.DimensionID.ToString() });                

                    modelDetails.Add(detail);
                }

            }

            model.MeasureDetails = modelDetails;

            return model;
        }

        /// <summary>
        /// Method to create and populate a CreateDimensionModel ready for the View to use
        /// </summary>
        /// <param name="datasetID">The ID of the Staging Table this Dimension is based off</param>
        /// <returns></returns>
        public CreateDimensionModel populateCreateDimensionModel(int datasetID)
        {
            CreateDimensionModel model = new CreateDimensionModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingDatasetID = datasetID;

            model.StagingColumnsForDimension = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
            {
                Text = x.ColumnName,
                Value = x.StagingColumnID.ToString()
            });

            return model;
        }

        /// <summary>
        /// Validate that the CreateDimensionModel is valid prior to
        /// submission to the database
        /// </summary>
        /// <param name="model">The model to be validated</param>
        /// <returns>Boolean indicating valid or not</returns>
        public Boolean isCreateDimensionModelValid(CreateDimensionModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            int count = context.Dimensions.Where(x => x.DimensionName.Equals(model.DimensionName)).Count();

            return count == 0; 
        }

        /// <summary>
        /// Method to create a Dimension based off the model returned from a View. This creates a Dimension
        /// and then populates values with those found in the specified Staging Table column
        /// </summary>
        /// <param name="model">The populated model passed back from the View</param>
        public void createDimension(CreateDimensionModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            Dimension newDim = context.Dimensions.Create();

            newDim.DimensionName = model.DimensionName;

            String stagingColumnName = context.StagingColumns.Single(x => x.StagingColumnID.Equals(model.DimColumnInStaging)).ColumnName;
            String stagingTableName = context.StagingColumns.Single(x => x.StagingColumnID.Equals(model.DimColumnInStaging)).StagingDataset.DatasetName;

            String sqlQuery = String.Format("select distinct {0} from {1}", stagingColumnName, stagingTableName);
            IEnumerable<String> stagingDimensionValues = context.Database.SqlQuery<String>(sqlQuery);

            foreach (String dimValue in stagingDimensionValues)
            {
                DimensionValue value = context.DimensionValues.Create();

                value.DimensionValue1 = dimValue;

                newDim.DimensionValues.Add(value);
            }

            context.Dimensions.Add(newDim);

            context.SaveChanges();

            context.Dispose();
        }


        /// <summary>
        /// Method to return the model which is used to map the Staging Dimension Values to Dimension Values
        /// in the database. This is used in the second stage in the Create Measure process. 
        /// </summary>
        /// <param name="mappings">The mappings between the Staging Dimension Column and a Dimension in the Database. This is passed in as a Tuple where the first item is a Staging Column ID and the second item a Dimension ID</param>
        /// <param name="stagingTableName">The name of the staging table being used</param>
        /// <param name="measureName">The name of the measure to be created</param>
        /// <param name="measureColumnStagingID">The ID of the column used for the Measure values in Staging</param>
        /// <param name="geographyColumnID">The ID of the colum used for the Geography value in Staging</param>
        /// <returns></returns>
        public MeasureValueModel populateMeasureValueModel(IEnumerable<Tuple<int, int>> mappings, String stagingTableName, String measureName, int? measureColumnStagingID, int geographyColumnID)
        {
            List<MeasureValueModel> retVal = new List<MeasureValueModel>();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            MeasureValueModel model = new MeasureValueModel();

            if (measureColumnStagingID.HasValue)
            {
                model.MeasureStagingColumnName = context.StagingColumns.Single(x => x.StagingColumnID.Equals(measureColumnStagingID.Value)).ColumnName;
                model.UseMeasureColumn = true;
            }
            else
            {
                model.UseMeasureColumn = false;
            }
            model.StagingGeographyColumn = context.StagingColumns.Single(x => x.StagingColumnID.Equals(geographyColumnID)).ColumnName;
            model.MeasureName = measureName;
            model.StagingTableName = stagingTableName;

            List<MeasureValueDetailModel> detailModels = new List<MeasureValueDetailModel>();

            foreach (Tuple<int, int> tuple in mappings)
            {
                
                String dimColumnInStaging = context.StagingColumns.Single(x => x.StagingColumnID.Equals(tuple.Item1)).ColumnName;

                String sqlQuery = String.Format("select distinct {0} from {1}", dimColumnInStaging, stagingTableName);

                IEnumerable<String> stagingMeasureValues = context.Database.SqlQuery<String>(sqlQuery);


                foreach (DimensionValue dim in context.DimensionValues.Where(x => x.DimensionID.Equals(tuple.Item2)))
                {
                    MeasureValueDetailModel modelDetail = new MeasureValueDetailModel();

                    modelDetail.DimensionID = tuple.Item2;
                    modelDetail.StagingDimensionValues = stagingMeasureValues.Select(x => new SelectListItem { Text = x, Value = x, Selected = dim.DimensionValue1.Equals(x) });

                    modelDetail.DimValue = dim.DimensionValue1;
                    modelDetail.DimValueID = dim.DimensionValueID;

                    modelDetail.DimColumnInStaging = dimColumnInStaging;

                    detailModels.Add(modelDetail);
                }                                
            }

            model.MeasureValueDetails = detailModels;
           
            return model;
        }

        /// <summary>
        /// Validate that the CreateMeasureModel is valid prior to
        /// submission to the database
        /// </summary>
        /// <param name="model">The model to be validated</param>
        /// <returns>Boolean indicating valid or not</returns>
        public Boolean isCreateMeasureModelValid(CreateMeasureModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            int count = context.Facts.Where(x => x.FactName.Equals(model.MeasureName)).Count();

            return count == 0;
        }

        /// <summary>
        /// Method to create the measure in the database. This just creates the basic Measure information and 
        /// does not add values under the Measure. This process is based off creating SQL dynamically based on
        /// a template file.
        /// </summary>
        /// <param name="model">The populated model containing the data needed to create the Measure</param>
        public void createMeasure(CreateMeasureModel model)
        {
            InsertMeasureTemplate template = new InsertMeasureTemplate();

            template.FactName = model.MeasureName;

            template.DimensionIDs = model.MeasureDetails.Where(x=> x.DimensionValueID.HasValue).Select(x => x.DimensionValueID.Value).OrderByDescending(x => x);

            String output = template.TransformText();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            context.Database.ExecuteSqlCommand(output);

            context.Dispose();
        }

        /// <summary>
        /// Method which populates the Measure Values into the database based off the populated model passed back
        /// from the View. This Process differentiates based on the number of Dimensions present in the model
        /// but in all cases a Template file is used to generate dynamic SQL. After the values are added a Default View is created
        /// which can be used to query the data.
        /// </summary>
        /// <param name="model">The populated model returned from the view that contains all the data needed to add the Measure Values to the Measure</param>
        public void createMeasureValues(MeasureValueModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            IEnumerable<int> dimIDs = model.MeasureValueDetails.Select(x => x.DimensionID).Distinct();

            if (dimIDs.Count() > 2)
            {
                //recurse
            }
            else if (dimIDs.Count() == 2)
            {
                //cross product


                int maxID = dimIDs.Max();
                int minID = dimIDs.Min();

                foreach (MeasureValueDetailModel maxItem in model.MeasureValueDetails.Where(x => x.DimensionID.Equals(maxID)))
                {
                    foreach (MeasureValueDetailModel minItem in model.MeasureValueDetails.Where(y => y.DimensionID.Equals(minID)))
                    {
                        MeasureLoadingModel xxx = new MeasureLoadingModel();

                        xxx.Dimensions = new List<DimensionModel>();
                        xxx.StagingDimensions = new List<StagingDimensionModel>();

                        xxx.Dimensions.Add(new DimensionModel() { DimensionID = maxItem.DimensionID, DimensionValue = maxItem.DimValue });
                        xxx.Dimensions.Add(new DimensionModel() { DimensionID = minItem.DimensionID, DimensionValue = minItem.DimValue });


                        xxx.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnName = maxItem.DimColumnInStaging, StagingColumnValue = maxItem.DimValueInStaging });
                        xxx.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnName = minItem.DimColumnInStaging, StagingColumnValue = minItem.DimValueInStaging });

                        InsertMeasureValuesTemplate template = new InsertMeasureValuesTemplate();

                        template.UseMeasureColumn = model.UseMeasureColumn;
                        template.StagingTableName = model.StagingTableName;
                        template.StagingGeographyColumn = model.StagingGeographyColumn;
                        template.MeasureName = model.MeasureName;
                        template.MeasureColumnName = model.MeasureStagingColumnName;
                        template.Details = xxx;
                        
                        String output = template.TransformText();                        

                        context.Database.ExecuteSqlCommand(output);
                    }
                }

            }
            else
            {
                foreach (MeasureValueDetailModel maxItem in model.MeasureValueDetails)
                {                   
                    MeasureLoadingModel xxx = new MeasureLoadingModel();

                    xxx.Dimensions = new List<DimensionModel>();
                    xxx.StagingDimensions = new List<StagingDimensionModel>();

                    xxx.Dimensions.Add(new DimensionModel() { DimensionID = maxItem.DimensionID, DimensionValue = maxItem.DimValue });

                    xxx.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnName = maxItem.DimColumnInStaging, StagingColumnValue = maxItem.DimValueInStaging });

                    InsertMeasureValuesTemplate template = new InsertMeasureValuesTemplate();

                    template.UseMeasureColumn = model.UseMeasureColumn;
                    template.StagingTableName = model.StagingTableName;
                    template.StagingGeographyColumn = model.StagingGeographyColumn;
                    template.MeasureName = model.MeasureName;
                    template.MeasureColumnName = model.MeasureStagingColumnName;
                    template.Details = xxx;

                    String output = template.TransformText();

                    context.Database.ExecuteSqlCommand(output);                    
                }
            }

            IEnumerable<String> dimensions = context.Dimensions.Where(y => dimIDs.Contains(y.DimensionID)).Select(x => x.DimensionName);

            CreateDefaultViewTemplate viewTemplate = new CreateDefaultViewTemplate();
            viewTemplate.DimensionNames = dimensions;
            viewTemplate.FactName = model.MeasureName;
            viewTemplate.AggregateQuery = !model.UseMeasureColumn;

            String viewOutput = viewTemplate.TransformText();

            DataView newView = context.DataViews.Create();
            newView.ViewName = model.MeasureName;

            newView.DataViewColumns.Add(new DataViewColumn() { ColumnName = "FactCount" });
            newView.DataViewColumns.Add(new DataViewColumn() { ColumnName = "LsoaName" });
            newView.DataViewColumns.Add(new DataViewColumn() { ColumnName = "LoadReference" });            

            foreach (String column in dimensions)
            {
                newView.DataViewColumns.Add(new DataViewColumn() { ColumnName = column });
            }

            context.DataViews.Add(newView);

            context.SaveChanges();

            context.Database.ExecuteSqlCommand(viewOutput);

            context.Dispose();

        }                     
    }
}