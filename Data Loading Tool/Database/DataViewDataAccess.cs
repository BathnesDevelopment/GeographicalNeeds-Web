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
        public IEnumerable<DataViewModel> getViews()
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            IEnumerable<DataViewModel> models = context.DataViews.Select(x => new DataViewModel() { DataViewID = x.DataViewID, DataViewName = x.ViewName });

            return models;
        }

        public DataViewDetailModel getViewData(int viewID)
        {
            DataViewDetailModel model = new DataViewDetailModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.ViewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            model.ViewData = getViewDataDetail(viewID);

            return model;
        }


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

        public CreateViewModel getCreateViewModel()
        {
            CreateViewModel model = new CreateViewModel();
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            model.Measures = context.Facts.ToList().Select(x => new SelectListItem() { Text = x.FactName, Value = x.FactID.ToString() });

            return model;
        }

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
                    retVal.Add(model);
                }
            }

            return retVal;
        }

        public CreateViewDimensionModel getViewDimensionModels(int dimensionID)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            CreateViewDimensionModel model = new CreateViewDimensionModel();

            model.DimensionValues = context.DimensionValues.Where(x => x.DimensionID.Equals(dimensionID)).ToList().Select(x => new SelectListItem() { Text = x.DimensionValue1, Value = x.DimensionValueID.ToString() });

            model.DimensionName = context.Dimensions.Single(x => x.DimensionID.Equals(dimensionID)).DimensionName;


            return model;
        }

        public void createBespokeView(CreateViewCompleteModel model)
        {
            Geographical_NeedsEntities context = new Geographical_NeedsEntities();

            DataView newView = context.DataViews.Create();
            newView.ViewName = model.ViewName;

            DataViewColumn lsoaColumn = context.DataViewColumns.Create();
            lsoaColumn.ColumnName = "LsoaName";
            newView.DataViewColumns.Add(lsoaColumn);

            foreach (CreateViewMeasureDimensionModel measure in model.Measures)
            {
                DataViewColumn newColumn = context.DataViewColumns.Create();
                newColumn.ColumnName = String.Format("{0} Count", measure.MeasureName);
                newView.DataViewColumns.Add(newColumn);
            }

            context.DataViews.Add(newView);

            CreateCustomViewTemplate template = new CreateCustomViewTemplate();
            template.model = model;

            String output = template.TransformText();

            context.Database.ExecuteSqlCommand(output);

            context.SaveChanges();

            context.Dispose();
        }

    }
}