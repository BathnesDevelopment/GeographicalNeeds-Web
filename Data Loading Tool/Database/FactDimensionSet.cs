//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data_Loading_Tool.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class FactDimensionSet
    {
        public FactDimensionSet()
        {
            this.FactDimensionMappings = new HashSet<FactDimensionMapping>();
            this.FactInstances = new HashSet<FactInstance>();
        }
    
        public int FactDimensionSetID { get; set; }
        public int FactID { get; set; }
        public string DimString { get; set; }
    
        public virtual Fact Fact { get; set; }
        public virtual ICollection<FactDimensionMapping> FactDimensionMappings { get; set; }
        public virtual ICollection<FactInstance> FactInstances { get; set; }
    }
}