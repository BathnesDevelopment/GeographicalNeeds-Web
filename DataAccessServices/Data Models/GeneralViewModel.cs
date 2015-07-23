using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;


namespace DataAccessServices.Data_Models
{
    [DataContract]
    public class GeneralViewModel
    {
        [DataMember]
        public DataTable ResultSet { get; set; }        
    }
}