﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RealEstateDeveloperDBEntities : DbContext
    {
        public RealEstateDeveloperDBEntities()
            : base("name=RealEstateDeveloperDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Apartments> Apartments { get; set; }
        public virtual DbSet<Buildings> Buildings { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ConstructionSchedule> ConstructionSchedule { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<MaintenanceRequests> MaintenanceRequests { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Rental> Rental { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
