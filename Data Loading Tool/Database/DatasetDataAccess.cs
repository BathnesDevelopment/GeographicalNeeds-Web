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
        /// DIMENSIONS 
        
        /// <summary>
        /// Method to create and populate a CreateDimensionModel ready for the View to use
        /// </summary>
        /// <param name="datasetID">The ID of the Staging Table this Dimension is based off</param>
        /// <returns></returns>
        public CreateDimensionModel PopulateCreateDimensionModel(int datasetID)
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
        public Boolean IsCreateDimensionModelValid(CreateDimensionModel model)
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
        public void CreateDimension(CreateDimensionModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();


            Dimension newDim = context.Dimensions.Create();

            newDim.DimensionName = model.DimensionName;

            DimensionSet newSet = context.DimensionSets.Create();
            newSet.DimensionSetName = String.Format("By {0}", model.DimensionName);

            DimensionSetMember newMember = context.DimensionSetMembers.Create();

            newDim.DimensionSetMembers.Add(newMember);
            newSet.DimensionSetMembers.Add(newMember);

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
            context.DimensionSets.Add(newSet);
            context.DimensionSetMembers.Add(newMember);

            context.SaveChanges();

            context.Dispose();
        }

        ///MEASURES

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

            model.GeographyTypes = context.GeographyTypes.Select(x => new SelectListItem()
                                                                                        {
                                                                                            Text = x.GeographyType1,
                                                                                            Value = x.GeographyTypeID.ToString()
                                                                                        });

            model.DimensionsForMeasureBreakdown = context.Dimensions.Select(x => new SelectListItem
            {
                Text = x.DimensionName,
                Value = x.DimensionID.ToString()
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
        /// Method to return the model which is used to map the Staging Dimension Values to Dimension Values
        /// in the database. This is used in the second stage in the Create Measure process. 
        /// </summary>
        /// <param name="mappings">The mappings between the Staging Dimension Column and a Dimension in the Database. This is passed in as a Tuple where the first item is a Staging Column ID and the second item a Dimension ID</param>
        /// <param name="stagingTableName">The name of the staging table being used</param>
        /// <param name="measureName">The name of the measure to be created</param>
        /// <param name="measureColumnStagingID">The ID of the column used for the Measure values in Staging</param>
        /// <param name="geographyColumnID">The ID of the colum used for the Geography value in Staging</param>
        /// <returns></returns>
        public MeasureValueModel populateMeasureValueModel(IEnumerable<Tuple<int, int>> mappings, String stagingTableName, String measureName, int? measureColumnStagingID, int geographyColumnID, int geographyTypeID)
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

            model.GeographyTypeID = geographyTypeID;
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

            int count = context.Measures.Where(x => x.MeasureName.Equals(model.MeasureName)).Count();

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
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            Measure newMeasure = context.Measures.Create();
            newMeasure.MeasureName = model.MeasureName;

            createDimSets(model.selectedDimensions);

            IEnumerable<DimensionSet> dimSets = context.Dimensions.Where(x => model.selectedDimensions.Contains(x.DimensionID)).SelectMany(x => x.DimensionSetMembers).Select(x => x.DimensionSet).ToList();
            IEnumerable<DimensionSet> dimSetsForExclusion = context.Dimensions.Where(x => !model.selectedDimensions.Contains(x.DimensionID)).SelectMany(x => x.DimensionSetMembers).Select(x => x.DimensionSet).ToList();

            IEnumerable<DimensionSet> dimSetsFinal = dimSets.Where(x => !dimSetsForExclusion.Select(y => y.DimensionSetID).Contains(x.DimensionSetID)).Distinct();

            foreach (DimensionSet set in dimSetsFinal)
            {
                MeasureBreakdown newBreakdown = context.MeasureBreakdowns.Create();
                newBreakdown.MeasureBreakdownName = String.Format("{0} {1}", model.MeasureName, set.DimensionSetName);
                newBreakdown.DimensionSetID = set.DimensionSetID;
                newMeasure.MeasureBreakdowns.Add(newBreakdown);
            }

            context.Measures.Add(newMeasure);

            context.SaveChanges();

            context.Dispose();
        }

        private void createDimSets(IEnumerable<int> dimIDs)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();
            List<DimensionSet> allNewSets = new List<DimensionSet>();

            foreach (int dimID in dimIDs)
            {
                Dimension dim = context.Dimensions.Single(x => x.DimensionID.Equals(dimID));



                foreach (DimensionSet set in allNewSets)
                {
                    DimensionSet derivedSet = context.DimensionSets.Create();
                    derivedSet.DimensionSetName = String.Format("{0} and {1}", set.DimensionSetName, dim.DimensionName);
                    context.DimensionSets.Add(derivedSet);

                    DimensionSetMember newDimMember = context.DimensionSetMembers.Create();
                    newDimMember.DimensionSet = derivedSet;
                    newDimMember.Dimension = dim;
                    context.DimensionSetMembers.Add(newDimMember);

                    foreach (DimensionSetMember dimSetMember in set.DimensionSetMembers)
                    {
                        DimensionSetMember derivedMember = context.DimensionSetMembers.Create();

                        derivedMember.Dimension = dimSetMember.Dimension;

                        derivedSet.DimensionSetMembers.Add(derivedMember);

                        context.DimensionSetMembers.Add(derivedMember);
                    }

                    foreach (DimensionSetCombination combination in set.DimensionSetCombinations)
                    {
                        foreach (DimensionValue dimValue in dim.DimensionValues)
                        {
                            DimensionSetCombination derivedCombination = context.DimensionSetCombinations.Create();
                            derivedCombination.DimensionSetCombinationName = String.Format("{0} | {1}: {2}", combination.DimensionSetCombinationName, dim.DimensionName, dimValue.DimensionValue1);
                            derivedCombination.DimensionSet = derivedSet;

                            context.DimensionSetCombinations.Add(derivedCombination);

                            DimensionSetCombinationMember newCombinationMember = context.DimensionSetCombinationMembers.Create();
                            newCombinationMember.DimensionSetCombination = derivedCombination;
                            newCombinationMember.DimensionValue = dimValue;

                            context.DimensionSetCombinationMembers.Add(newCombinationMember);
                            
                            foreach (DimensionSetCombinationMember dimSetCombinationMember in combination.DimensionSetCombinationMembers)
                            {
                                DimensionSetCombinationMember derivedCombinationMember = context.DimensionSetCombinationMembers.Create();
                                derivedCombinationMember.DimensionSetCombination = derivedCombination;
                                derivedCombinationMember.DimensionValue = dimSetCombinationMember.DimensionValue;

                                context.DimensionSetCombinationMembers.Add(derivedCombinationMember);    

                            }
                        }
                    }
                }

                String newDimSetName = String.Format("By {0}", dim.DimensionName);

                if (context.DimensionSets.Count(x => x.DimensionSetName.Equals(newDimSetName)) == 0)
                {
                    DimensionSet newSet = context.DimensionSets.Create();
                    newSet.DimensionSetName = String.Format("By {0}", dim.DimensionName);

                    DimensionSetMember newMember = context.DimensionSetMembers.Create();

                    dim.DimensionSetMembers.Add(newMember);
                    newSet.DimensionSetMembers.Add(newMember);

                    foreach (DimensionValue dimValue in dim.DimensionValues)
                    {
                        DimensionSetCombination newCombination = context.DimensionSetCombinations.Create();
                        newCombination.DimensionSet = newSet;
                        newCombination.DimensionSetCombinationName = String.Format("{0}: {1}", dim.DimensionName, dimValue.DimensionValue1);

                        DimensionSetCombinationMember newCombinationMember = context.DimensionSetCombinationMembers.Create();
                        newCombinationMember.DimensionValue = dimValue;
                        newCombinationMember.DimensionSetCombination = newCombination;
                        
                        context.DimensionSetCombinations.Add(newCombination);
                        context.DimensionSetCombinationMembers.Add(newCombinationMember);
                    }
                        
                    allNewSets.Add(newSet);
                    
                }

            }

            context.SaveChanges();
            context.Dispose();
        }

        public void createMeasureValues(MeasureValueModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            var breakdowns = context.Measures.Single(x => x.MeasureName.Equals(model.MeasureName)).MeasureBreakdowns;

            foreach (var breakdown in breakdowns)
            {
                var combinations = breakdown.DimensionSet.DimensionSetCombinations;

                foreach (var combination in combinations)
                {
                    var values = combination.DimensionSetCombinationMembers.Select(x => x.DimensionValue);

                    List<String> whereClauses = new List<string>();

                    foreach (var value in values)
	                {
                        var detail = model.MeasureValueDetails.Single(x => x.DimValueID.Equals(value.DimensionValueID));
                        whereClauses.Add(String.Format("[{0}] = '{1}'", detail.DimColumnInStaging, detail.DimValueInStaging));
	                }
                    String whereClause = String.Format("Where {0}", String.Join(" AND ", whereClauses));

                    InsertMeasureValuesTemplate template = new InsertMeasureValuesTemplate();

                    template.DimensionSetCombinationID = combination.DimensionSetCombinationID;
                    template.GeographyTypeID = model.GeographyTypeID;
                    template.MeasureBreakdownID = breakdown.MeasureBreakdownID;
                    template.MeasureColumnName = model.MeasureStagingColumnName;
                    template.StagingGeographyColumn = model.StagingGeographyColumn;
                    template.StagingTableName = model.StagingTableName;
                    template.UseMeasureColumn = model.UseMeasureColumn;
                    template.WhereClause = whereClause;

                    String output = template.TransformText();

                    context.Database.ExecuteSqlCommand(output);
                }
            }

            context.Dispose();
        }

       
    }
}