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
    
    public partial class DimensionToFact
    {
        public int DimensionToFactID { get; set; }
        public int FactID { get; set; }
        public int DimensionID { get; set; }
    
        public virtual Dimension Dimension { get; set; }
        public virtual Fact Fact { get; set; }
    }
}
