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
    
    public partial class Payments
    {
        public int PaymentID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
    
        public virtual Clients Clients { get; set; }
    }
}