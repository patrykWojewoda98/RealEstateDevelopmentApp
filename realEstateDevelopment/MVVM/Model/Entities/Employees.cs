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
    
    public partial class Employees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            this.EmployeeProjects = new HashSet<EmployeeProjects>();
            this.HistoryOfChanges = new HashSet<HistoryOfChanges>();
        }
    
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeProjects> EmployeeProjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryOfChanges> HistoryOfChanges { get; set; }
    }
}
