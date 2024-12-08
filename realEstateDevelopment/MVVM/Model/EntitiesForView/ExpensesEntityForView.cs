using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ExpensesEntityForView
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string ExpenseType { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
