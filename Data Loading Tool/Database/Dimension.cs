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
    
    public partial class Dimension
    {
        public Dimension()
        {
            this.DimensionValues = new HashSet<DimensionValue>();
            this.DimensionSetMembers = new HashSet<DimensionSetMember>();
        }
    
        public int DimensionID { get; set; }
        public string DimensionName { get; set; }
    
        public virtual ICollection<DimensionValue> DimensionValues { get; set; }
        public virtual ICollection<DimensionSetMember> DimensionSetMembers { get; set; }
    }
}
