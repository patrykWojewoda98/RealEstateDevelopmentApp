using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class RevenuesEntityForView
    {
        public int Id { get; set; }
        public string ProjectName {  get; set; }
        public string Address { get; set; }
        public string RevenueType { get; set; }
        public decimal RevenueAmount { get; set; }
        public DateTime RevenueDate { get; set; }
    }
}
