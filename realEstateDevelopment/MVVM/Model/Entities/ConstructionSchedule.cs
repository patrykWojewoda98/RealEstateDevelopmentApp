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
    
    public partial class ConstructionSchedule
    {
        public int ScheduleID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string ConstructionPhase { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Status { get; set; }
    
        public virtual Projects Projects { get; set; }
    }
}
