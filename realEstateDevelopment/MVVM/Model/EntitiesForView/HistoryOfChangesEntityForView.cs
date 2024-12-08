using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class HistoryOfChangesEntityForView
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string Operation { get; set; }
        public DateTime DateAndTimeOfChange {  get; set; }
    }
}
