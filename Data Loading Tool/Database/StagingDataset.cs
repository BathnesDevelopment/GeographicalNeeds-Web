//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data_Loading_Tool.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class StagingDataset
    {
        public StagingDataset()
        {
            this.StagingColumns = new HashSet<StagingColumn>();
        }
    
        public int StagingDatasetID { get; set; }
        public string DatasetName { get; set; }
    
        public virtual ICollection<StagingColumn> StagingColumns { get; set; }
    }
}
