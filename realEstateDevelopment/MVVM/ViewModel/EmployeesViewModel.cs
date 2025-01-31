using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Data.Entity;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class EmployeesViewModel : LoadAllViewModel<EmployeesEntityForView>
    {
        #region Properties
        private EmployeesEntityForView _selectedItem;
        public EmployeesEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion

        #region Commands
        public RealyCommand OpenAddNewEmployeeCommand { get; set; }
        #endregion
        public event Action AddNewEmployeeRequested;
        public RealyCommand DeleteSelectedCommand { get; set; }
        #region
        public EmployeesViewModel()
            : base()
        {
            OpenAddNewEmployeeCommand = new RealyCommand(o =>
            {
                AddNewEmployeeRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
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

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is EmployeesEntityForView selected)
            {
                var modal = new DeleteEmployeeModalView();
                DeleteEmployeeModalViewModel deleteEmployeeModalViewModel = new DeleteEmployeeModalViewModel(
                                            realEstateEntities.Employees.First(e => e.EmployeeID == SelectedItem.EmployeeId));
                deleteEmployeeModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteEmployeeModalViewModel;
                modal.Show();

            }
        }


        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null; // Polecenie dostępne tylko, jeśli coś jest zaznaczone.
        }
        #endregion
    }
}
