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
    
    public partial class Fact
    {
        public Fact()
        {
            this.FactDimensionSets = new HashSet<FactDimensionSet>();
        }
    
        public int FactID { get; set; }
        public string FactName { get; set; }
    
        public virtual ICollection<FactDimensionSet> FactDimensionSets { get; set; }
    }
}