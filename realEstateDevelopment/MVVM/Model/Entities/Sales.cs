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
    
    public partial class Sales
    {
        public int SaleID { get; set; }
        public int ClientID { get; set; }
        public int ApartmentID { get; set; }
        public decimal SalePrice { get; set; }
        public System.DateTime SaleDate { get; set; }
        public string Status { get; set; }
    
        public virtual Apartments Apartments { get; set; }
        public virtual Clients Clients { get; set; }
        public virtual Clients Clients1 { get; set; }
    }
}
