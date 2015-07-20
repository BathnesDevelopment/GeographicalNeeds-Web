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

        public GeneralDatasetModel populateInitialModel(int datasetID)
        {
            GeneralDatasetModel model = new GeneralDatasetModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.StagingTableName = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).DatasetName;

            MultiSelectList dimensions = new MultiSelectList(context.Dimensions.Select(x => new SelectListItem()
            {
                Text = x.DimensionName,
                Value = SqlFunctions.StringConvert((double)x.DimensionID).Trim()
                //Value = x.DimensionID.ToString()
            }), "Value", "Text");

            MultiSelectList stagingColumns = new MultiSelectList(context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
            {
                Text = x.ColumnName,
                Value = x.StagingColumnID.ToString()
            }), "Value", "Text");

            model.AvailableDimensions = dimensions;

            model.StagingColumnsForDimension = stagingColumns;

            //model.AvailableDimensions = context.Dimensions.Select(x => new SelectListItem() { 
            //                                                                                    Text = x.DimensionName, 
            //                                                                                    Value = SqlFunctions.StringConvert((double)x.DimensionID).Trim() 
            //                                                                                });

            //model.StagingColumnsForDimension = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
            //                                                                                                                                                        {
            //                                                                                                                                                            Text = x.ColumnName,
            //                                                                                                                                                            Value = x.StagingColumnID.ToString()
            //                                                                                                                                                        });

            model.StagingColumnsForMeasure = context.StagingDatasets.Single(x => x.StagingDatasetID.Equals(datasetID)).StagingColumns.Select(x => new SelectListItem()
                                                                                                                                                                    {
                                                                                                                                                                        Text = x.ColumnName,
                                                                                                                                                                        Value = x.StagingColumnID.ToString()
                                                                                                                                                                    });


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

            template.DimensionIDs = model.MeasureDetails.Where(x=> x.DimensionValueID.HasValue).Select(x => x.DimensionValueID.Value);

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

            String viewOutput = viewTemplate.TransformText();

            context.Database.ExecuteSqlCommand(viewOutput);

            context.Dispose();

        }

        
        public void createMeasureValuesTemp(MeasureLoadingModel calculatedModel)
        {
            InsertMeasureValuesTemplate template = new InsertMeasureValuesTemplate();

            template.UseMeasureColumn = false;
            template.StagingTableName = "StagingNCMP";
            template.StagingGeographyColumn = "Pupil_LSOA_2011";
            template.MeasureName = "ObesityTest";
            
            MeasureLoadingModel model = new MeasureLoadingModel();

            model.Dimensions = new List<DimensionModel>();

            model.Dimensions.Add(new DimensionModel() { DimensionID = 4, DimensionValue = "R" });
            model.Dimensions.Add(new DimensionModel() { DimensionID = 3, DimensionValue = "Obese" });

            model.StagingDimensions = new List<StagingDimensionModel>();
            model.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnValue = "R", StagingColumnName = "School_yr" });
            model.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnValue = "Obese", StagingColumnName = "Weight_Status_population" });

            template.Details = calculatedModel;

            String output = template.TransformText();

        }

        public void createMeasureValuesTemp2()
        {
            MeasureValueModel model = new MeasureValueModel();

            model.UseMeasureColumn = false;
            model.StagingTableName = "StagingNCMP";
            model.StagingGeographyColumn = "Pupil_LSOA_2011";
            model.MeasureName = "ObesityTest";

            List<MeasureValueDetailModel> details = new List<MeasureValueDetailModel>();

            details.Add(new MeasureValueDetailModel() { DimensionID = 4, DimValueInStaging = "R", DimValue = "R", DimColumnInStaging = "School_yr" });
            details.Add(new MeasureValueDetailModel() { DimensionID = 4, DimValueInStaging = "6", DimValue = "6", DimColumnInStaging = "School_yr" });
            details.Add(new MeasureValueDetailModel() { DimensionID = 3, DimValueInStaging = "Obese", DimValue = "Obese", DimColumnInStaging = "Weight_Status_population" });
            details.Add(new MeasureValueDetailModel() { DimensionID = 3, DimValueInStaging = "Underweight", DimValue = "Underweight", DimColumnInStaging = "Weight_Status_population" });
            details.Add(new MeasureValueDetailModel() { DimensionID = 3, DimValueInStaging = "Overweight", DimValue = "Overweight", DimColumnInStaging = "Weight_Status_population" });
            details.Add(new MeasureValueDetailModel() { DimensionID = 3, DimValueInStaging = "Healthy Weight", DimValue = "Healthy Weight", DimColumnInStaging = "Weight_Status_population" });

            model.MeasureValueDetails = details;


            MeasureLoadingModel xxx = new MeasureLoadingModel();


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

                foreach (MeasureValueDetailModel maxItem in details.Where(x => x.DimensionID.Equals(maxID)))
                {
                    foreach (MeasureValueDetailModel minItem in details.Where(y => y.DimensionID.Equals(minID)))
                    {
                        xxx.Dimensions = new List<DimensionModel>();
                        xxx.StagingDimensions = new List<StagingDimensionModel>();

                        xxx.Dimensions.Add(new DimensionModel() { DimensionID = maxItem.DimensionID, DimensionValue = maxItem.DimValue });
                        xxx.Dimensions.Add(new DimensionModel() { DimensionID = minItem.DimensionID, DimensionValue = minItem.DimValue });


                        xxx.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnName = maxItem.DimColumnInStaging, StagingColumnValue = maxItem.DimValueInStaging });
                        xxx.StagingDimensions.Add(new StagingDimensionModel() { StagingColumnName = minItem.DimColumnInStaging, StagingColumnValue = minItem.DimValueInStaging });

                        createMeasureValuesTemp(xxx);
                    }
                }

            }
            else 
            {
                ///return simple case
            }

            String yyy = xxx.ToString();

        }
    }
}