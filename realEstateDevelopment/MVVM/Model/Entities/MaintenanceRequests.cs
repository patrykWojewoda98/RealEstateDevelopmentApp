//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace realEstateDevelopment.MVVM.Model.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaintenanceRequests
    {
        public int RequestID { get; set; }
        public Nullable<int> ApartmentID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string Status { get; set; }
    
        public virtual Apartments Apartments { get; set; }
        public virtual Clients Clients { get; set; }
    }
}
