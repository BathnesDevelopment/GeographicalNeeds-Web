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

//            model.ViewData = getViewDataDetailFromService(viewID);
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
        /// Internal helper method to return the data in a View as a Data Table that can be added to 
        /// a View Model in the public method. This dynamically generates SQL to query the table.
        /// </summary>
        /// <param name="viewID">The ID of the Staging Table to be queried</param>
        /// <returns></returns>
        private DataTable getViewDataDetailFromService(int viewID)
        {
            DataTable dt;

            GeographicalNeedsService.GeographicalNeedsServiceClient service = new GeographicalNeedsService.GeographicalNeedsServiceClient();

            dt = service.GetViewData(viewID);

            return dt;
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
        /// A method to get the basic Create View model with drop down
        /// data populated
        /// </summary>
        /// <returns>The populated model</returns>
        public CreateViewModel getDefaultCreateViewModel()
        {
            CreateViewModel model = new CreateViewModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.GeographyTypes = context.GeographyTypes.Select(x => new SelectListItem() { Text = x.GeographyType1, Value = x.GeographyTypeID.ToString() });            

            return model;
        }

        /// <summary>
        /// A method to get the model which contains the 
        /// data to add a column to a View
        /// </summary>
        /// <returns>The populated model</returns>
        public CreateViewColumnModel getDefaultColumnModel()
        {
            CreateViewColumnModel model = new CreateViewColumnModel();

            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.Measures = context.Measures.Select(x => new SelectListItem() { Text = x.MeasureName, Value = x.MeasureID.ToString() });

            model.MeasureBreakdowns = new List<SelectListItem>();            

            model.DimensionValues= new List<SelectListItem>();         

            return model;
        }

        /// <summary>
        /// Contains the data passed back from the view which can then be added into the model dynamically
        /// </summary>
        /// <param name="measureID"> The ID of the measure used</param>
        /// <param name="measureBreakdownID"> The ID of the measure breakdown used</param>
        /// <param name="dimValueID"> The ID of the Dim combinations to use</param>
        /// <param name="newColumnName"> The name of the new column</param>
        /// <returns>A model to be added into the View</returns>
        public ViewColumnModel getColumnModel(int measureID, int measureBreakdownID, int dimValueID, String newColumnName)
        {
            ViewColumnModel model = new ViewColumnModel();
            
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.ColumnName = newColumnName;

            model.SelectedMeasureID = measureID;
            model.SelectedMeasure = context.Measures.Single(x => x.MeasureID.Equals(measureID)).MeasureName;

            model.SelectedMeasureBreakdownID = measureBreakdownID;
            model.SelectedMeasureBreakdown = context.MeasureBreakdowns.Single(x => x.MeasureBreakdownID.Equals(measureBreakdownID)).MeasureBreakdownName;

            model.SelectedDimensionValueID = dimValueID;
            model.SelectedDimensionValue = context.DimensionSetCombinations.Single(x => x.DimensionSetCombinationID.Equals(dimValueID)).DimensionSetCombinationName;

            return model;
        }

        /// <summary>
        /// Method to get a list of Measure Breakdowns that are under a Measure
        /// </summary>
        /// <param name="measureID">The ID of the measure</param>
        /// <returns>A set of Select List Items containing the Measure Breakdowns</returns>
        public IEnumerable<SelectListItem> getMeasureBreakdownsForMeasure(int measureID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            return context.MeasureBreakdowns.Where(x => x.MeasureID.Equals(measureID)).Select(x => new SelectListItem() { Text = x.MeasureBreakdownName, Value = x.MeasureBreakdownID.ToString() });            
        }

        /// <summary>
        /// Method to get a list of Dimension Set Combinations which are related
        /// to all the Measures Instance values under a breakdown
        /// </summary>
        /// <param name="measureBreadownID">The ID of the Measure Breakdown</param>
        /// <returns>A set of Select List Items containing the Dimension Set Combinations</returns>
        public IEnumerable<SelectListItem> getDimensionSetCombinationsForMeasureBreakdowns(int measureBreadownID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            return context.MeasureBreakdowns.Single(x => x.MeasureBreakdownID.Equals(measureBreadownID)).MeasureInstances.Select(x => x.DimensionSetCombination).Distinct().Select(x => new SelectListItem() { Text = x.DimensionSetCombinationName, Value = x.DimensionSetCombinationID.ToString() });
        }

        /// <summary>
        /// A method to create the View from the user entered data that
        /// has been specified through the web interface. It uses the template
        /// for a new View to generate the necessary SQL.
        /// </summary>
        /// <param name="model">The fully created model</param>
        public void CreateView(CreateViewModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            DataView newView = context.DataViews.Create();
            newView.ViewName = model.ViewName;

            DataViewColumn geogColumn = context.DataViewColumns.Create();
            geogColumn.ColumnName = context.GeographyTypes.Single(x => x.GeographyTypeID.Equals(model.SelectedGeographyType)).GeographyType1;
            geogColumn.DataView = newView;

            context.DataViewColumns.Add(geogColumn);

            foreach (var column in model.Columns)
            {
                DataViewColumn newColumn = context.DataViewColumns.Create();

                newColumn.ColumnName = column.ColumnName;

                newColumn.DataView = newView;

                context.DataViewColumns.Add(newColumn);
            }

            context.DataViews.Add(newView);
            
            CreateViewTemplate template = new CreateViewTemplate();

            template.Model = model;

            String output = template.TransformText();

            context.Database.ExecuteSqlCommand(output);

            context.SaveChanges();

            context.Dispose();
        }

    }
}