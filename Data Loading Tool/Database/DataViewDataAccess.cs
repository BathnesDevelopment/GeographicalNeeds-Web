using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Data_Loading_Tool.Models;

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

            GeographicalNeedsService.GeographicalNeedsServiceClient client = new GeographicalNeedsService.GeographicalNeedsServiceClient();

            model.ViewName = context.DataViews.Single(x => x.DataViewID.Equals(viewID)).ViewName;

            model.ViewData = client.GetViewData(viewID);
            return model;
        }

    }
}