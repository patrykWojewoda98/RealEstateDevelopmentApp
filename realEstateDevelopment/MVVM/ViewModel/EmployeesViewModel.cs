using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Data.Entity;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class EmployeesViewModel : LoadAllViewModel<EmployeesEntityForView>
    {

        #region Commands
        public RealyCommand OpenAddNewEmployeeCommand { get; set; }
        #endregion
        public event Action AddNewEmployeeRequested;
        #region
        public EmployeesViewModel()
            : base()
        {
            OpenAddNewEmployeeCommand = new RealyCommand(o =>
            {
                AddNewEmployeeRequested?.Invoke();
            });
        }
        #endregion
        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from e in realEstateEntities.Employees
                        select new EmployeesEntityForView
                        {
                            EmployeeId = e.EmployeeID,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Position = e.Position,
                            Department = e.Department,
                            Salary = e.Salary,
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<EmployeesEntityForView>(result);

        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
