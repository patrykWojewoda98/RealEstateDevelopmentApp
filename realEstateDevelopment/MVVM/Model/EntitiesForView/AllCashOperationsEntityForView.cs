using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class AllCashOperationsEntityForView
    {
     
        public int Id { get; set; } 
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
