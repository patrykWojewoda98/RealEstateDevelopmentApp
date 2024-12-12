using System.Collections.ObjectModel;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class EmployeesEntityForView
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public decimal? Salary { get; set; }
        public ObservableCollection <string> Projects { get; set; }
        public string SelectedProject { get; set; }
    }
}
