using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data_Loading_Tool.Models;

namespace Data_Loading_Tool.Templates
{
    /// <summary>
    /// A class used to attach properties to the 
    /// Insert Measure Values Template file
    /// </summary>
    public partial class InsertMeasureValuesTemplate
    {
        public Boolean UseMeasureColumn { get; set; }

        public String MeasureColumnName { get; set; }
                
        public String StagingGeographyColumn { get; set; }

        public String StagingTableName { get; set; }

        public String WhereClause { get; set; }
        
        public int GeographyTypeID { get; set; }

        public int MeasureBreakdownID { get; set; }

        public int DimensionSetCombinationID { get; set; }
    }
}