using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Data.Entity;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows.Input;

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

        private string _filterFirstName;
        public string FilterFirstName
        {
            get => _filterFirstName;
            set
            {
                _filterFirstName = value;
                OnPropertyChanged(() => FilterFirstName);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterSalaryFrom;
        public decimal? FilterSalaryFrom
        {
            get => _filterSalaryFrom;
            set
            {
                _filterSalaryFrom = value;
                OnPropertyChanged(() => FilterSalaryFrom);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterSalaryTo;
        public decimal? FilterSalaryTo
        {
            get => _filterSalaryTo;
            set
            {
                _filterSalaryTo = value;
                OnPropertyChanged(() => FilterSalaryTo);
                ApplyFiltersAsync();
            }
        }

        private string _filterPosition;
        public string FilterPosition
        {
            get => _filterPosition;
            set
            {
                _filterPosition = value;
                OnPropertyChanged(() => FilterPosition);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<EmployeesEntityForView> _filteredList;
        public ObservableCollection<EmployeesEntityForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        #endregion

        #region Commands
        public RealyCommand OpenAddNewEmployeeCommand { get; set; }
        #endregion
        public event Action AddNewEmployeeRequested;
        public RealyCommand DeleteSelectedCommand { get; set; }
        public ICommand ApplyFiltersCommand { get; set; }
        #region
        public EmployeesViewModel()
            : base()
        {
            _filterSalaryTo = realEstateEntities.Employees.Max(e => e.Salary);
            OpenAddNewEmployeeCommand = new RealyCommand(o =>
            {
                AddNewEmployeeRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
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
            var filtered = List.Where(e =>
                (string.IsNullOrEmpty(FilterFirstName) ||
                 (!string.IsNullOrEmpty(e.FirstName) && e.FirstName.Contains(FilterFirstName))) &&
                (!FilterSalaryFrom.HasValue || e.Salary >= FilterSalaryFrom.Value) &&
                (!FilterSalaryTo.HasValue || e.Salary <= FilterSalaryTo.Value) &&
                (string.IsNullOrEmpty(FilterPosition) || e.Position.Contains(FilterPosition))
            );

            FilteredList = new ObservableCollection<EmployeesEntityForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
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
