using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;

using Data_Loading_Tool.Models;
using Data_Loading_Tool.Templates;

namespace Data_Loading_Tool.Database
{
    public class DatasetDataAccess
    {
        public CreateMeasureModel populateCreateMeasureModel(int datasetID)
        {
            CreateMeasureModel model = new CreateMeasureModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

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

        public CreateDimensionModel populateCreateDimensionModel(int datasetID)
        {
            CreateDimensionModel model = new CreateDimensionModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingColumnsForDimension = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
            {
                Text = x.ColumnName,
                Value = x.StagingColumnID.ToString()
            });

            return model;
        }

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
                    modelDetail.StagingDimensionValues = stagingMeasureValues.Select(x => new SelectListItem { Text = x, Value = x });

                    modelDetail.DimValue = dim.DimensionValue1;
                    modelDetail.DimValueID = dim.DimensionValueID;

                    modelDetail.DimColumnInStaging = dimColumnInStaging;

                    detailModels.Add(modelDetail);
                }                                
            }

            model.MeasureValueDetails = detailModels;
           
            return model;
        }

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