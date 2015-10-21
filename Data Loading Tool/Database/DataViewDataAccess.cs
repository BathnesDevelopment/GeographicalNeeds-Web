using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

using Data_Loading_Tool.Models;
using Data_Loading_Tool.Templates;

namespace Data_Loading_Tool.Database
{
    public class DataViewDataAccess
    {
        /// <summary>
        /// Method which returns all the Data Views held in the Database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataViewModel> getViews()
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            IEnumerable<DataViewModel> models = context.DataViews.Select(x => new DataViewModel() { DataViewID = x.DataViewID, DataViewName = x.ViewName });

            return models;
        }

        /// <summary>
        /// A method to get back all the data held in a view and return a model which 
        /// can be used to display the data
        /// </summary>
        /// <param name="viewID">The database ID of the View to be returned</param>
        /// <returns></returns>
        public DataViewDetailModel getViewData(int viewID)
        {
            DataViewDetailModel model = new DataViewDetailModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.ViewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            model.ViewData = getViewDataDetail(viewID);

            return model;
        }

        /// <summary>
        /// Internal helper method to return the data in a View as a Data Table that can be added to 
        /// a View Model in the public method. This dynamically generates SQL to query the table.
        /// </summary>
        /// <param name="viewID">The ID of the Staging Table to be queried</param>
        /// <returns></returns>
        private DataTable getViewDataDetail(int viewID)
        {
            DataTable dt;
            SqlDataAdapter sda;

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            String columns = String.Join(",", context.DataViews.Single(x => x.DataViewID.Equals(viewID)).DataViewColumns.Select(x => String.Format("[{0}]", x.ColumnName)));
            String viewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            String sql = String.Format("Select {1} from [{0}]", viewName, columns);

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

            return dt;
        }

        /// <summary>
        /// Method used to get get back the intial model used in Creating a custom View
        /// </summary>
        /// <returns></returns>
        public CreateViewModel getCreateViewModel()
        {
            CreateViewModel model = new CreateViewModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.Measures = context.Facts.ToList().Select(x => new SelectListItem() { Text = x.FactName, Value = x.FactID.ToString() });

            return model;
        }

        /// <summary>
        /// Validate that the Create Data View Model is valid prior to
        /// submission to the database
        /// </summary>
        /// <param name="model">The model to be validated</param>
        /// <returns>Boolean indicating valid or not</retu
        public Boolean isCreateViewModelValid(CreateViewModel model) 
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            int count = context.DataViews.Where(x => x.ViewName.Equals(model.ViewName)).Count();

            return count == 0;
        }

        /// <summary>
        /// Method which gets back the Models used to select Dimensions within the selected Measures when creating a custom View. 
        /// </summary>
        /// <param name="measureIDs">The Measures that have previously been selected within the create View process</param>
        /// <returns></returns>
        public IEnumerable<CreateViewMeasureModel> getViewMeasureModels(IEnumerable<int> measureIDs)
        {
            List<CreateViewMeasureModel> models = new List<CreateViewMeasureModel>();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            foreach (int id in measureIDs)
            {
                CreateViewMeasureModel model = new CreateViewMeasureModel();

                model.MeasureName = context.Facts.Single(x => x.FactID.Equals(id)).FactName;

                model.Dimensions = context.Facts.Single(x => x.FactID.Equals(id)).DimensionToFacts.Select(x => x.Dimension).Select(x => new SelectListItem() {Text = x.DimensionName, Value = x.DimensionID.ToString()});

                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Method which converts a View model for the Create View process into a Database orientated model. 
        /// </summary>
        /// <param name="models">The view models being passed in to be converted</param>
        /// <returns></returns>
        public IEnumerable<CreateViewMeasureDimensionModel> convertMeasureViewModelsToDBModels(List<CreateViewMeasureModel> models)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();
            
            List<CreateViewMeasureDimensionModel> retVal = new List<CreateViewMeasureDimensionModel>();

            foreach (CreateViewMeasureModel item in models)
            {
                CreateViewMeasureDimensionModel model = new CreateViewMeasureDimensionModel();

                model.MeasureID = context.Facts.Single(x => x.FactName.Equals(item.MeasureName)).FactID;
                model.MeasureName = item.MeasureName;

                retVal.Add(model);
            }

            return retVal;
        }

        /// <summary>
        /// Method to convert a set of models used in the secondary stages of the Create View process to 
        /// models which are database orientated.
        /// </summary>
        /// <param name="models">The models to be converted</param>
        /// <param name="measureID">The measure that these dimensions refer to</param>
        /// <returns></returns>
        public IEnumerable<CreateViewDimValueModel> convertDimensionViewModelsToDBModels(List<CreateViewDimensionModel> models, int measureID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            List<CreateViewDimValueModel> retVal = new List<CreateViewDimValueModel>();

            String measureName = context.Facts.Single(x => x.FactID.Equals(measureID)).FactName;            

            foreach (CreateViewDimensionModel item in models)
            {
                if (item.MeasureName.Equals(measureName))
                {                
                    CreateViewDimValueModel model = new CreateViewDimValueModel();

                    model.DimensionID = context.Dimensions.Single(x => x.DimensionName.Equals(item.DimensionName)).DimensionID;

                    model.DimensionValueIDs = item.SelectedDimensionValueIDs;

                    model.DimensionValues = context.DimensionValues.Where(x => model.DimensionValueIDs.Contains(x.DimensionValueID)).Select(x => x.DimensionValue1);

                    retVal.Add(model);
                }
            }            

            return retVal;
        }

        /// <summary>
        /// Method to get back a single model which allows for selection of Dimension Values as part 
        /// of the process to create a custom View
        /// </summary>
        /// <param name="dimensionID">The ID of the Dimension to get the Values from</param>
        /// <returns></returns>
        public CreateViewDimensionModel getViewDimensionModels(int dimensionID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            CreateViewDimensionModel model = new CreateViewDimensionModel();

            model.DimensionValues = context.DimensionValues.Where(x => x.DimensionID.Equals(dimensionID)).ToList().Select(x => new SelectListItem() { Text = x.DimensionValue1, Value = x.DimensionValueID.ToString() });

            model.DimensionName = context.Dimensions.Single(x => x.DimensionID.Equals(dimensionID)).DimensionName;

            model.DimensionID = dimensionID;

            return model;
        }

        /// <summary>
        /// The main method to create a Custom View based on the data in the model. 
        /// This is created using a text Template to generate dynamic SQL
        /// </summary>
        /// <param name="model">Populated model containing the data necessary to create the view</param>
        public void createBespokeView(CreateViewCompleteModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            DataView newView = context.DataViews.Create();
            newView.ViewName = model.ViewName;

            DataViewColumn geographyColumn = context.DataViewColumns.Create();
            geographyColumn.ColumnName = "GeographyName";
            newView.DataViewColumns.Add(geographyColumn);


            foreach (CreateViewMeasureDimensionModel measure in model.Measures)
            {
                if (measure.DimensionValues.Count() == 0)
                {
                    DataViewColumn newColumn = context.DataViewColumns.Create();
                    newColumn.ColumnName = String.Format("{0} - All", measure.MeasureName);
                    newView.DataViewColumns.Add(newColumn);
                }
                else
                {
                    foreach (String dimValue in measure.DimensionValues)
                    {
                        DataViewColumn newColumn = context.DataViewColumns.Create();
                        newColumn.ColumnName = String.Format("{0} - {1} Count", measure.MeasureName, dimValue);
                        newView.DataViewColumns.Add(newColumn);                        
                    }
                }

                DataViewColumn referenceColumn = context.DataViewColumns.Create();
                referenceColumn.ColumnName = String.Format("{0} - Loading reference", measure.MeasureName);
                newView.DataViewColumns.Add(referenceColumn);
            }

            context.DataViews.Add(newView);

            IEnumerable<int> measureIDs = model.Measures.Select(x => x.MeasureID);

            int geographyAggregationLevel = context.Facts.Where(x => measureIDs.Contains(x.FactID)).Max(x => x.GeographyTypeID);


            CreateCustomViewTemplate template = new CreateCustomViewTemplate();
            template.model = model;
            template.GeographyAggregationlevel = geographyAggregationLevel;

            String output = template.TransformText();            

            context.Database.ExecuteSqlCommand(output);

            context.SaveChanges();

            context.Dispose();
        }

    }
}