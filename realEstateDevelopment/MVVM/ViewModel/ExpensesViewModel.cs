using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ExpensesViewModel : LoadAllViewModel<ExpensesEntityForView>
    {
        #region Properties
        private ExpensesEntityForView _selectedItem;
        public ExpensesEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private string _filterProjectName;
        public string FilterProjectName
        {
            get => _filterProjectName;
            set
            {
                _filterProjectName = value;
                OnPropertyChanged(() => FilterProjectName);
                ApplyFiltersAsync();
            }
        }

        private string _filterExpenseType;
        public string FilterExpenseType
        {
            get => _filterExpenseType;
            set
            {
                _filterExpenseType = value;
                OnPropertyChanged(() => FilterExpenseType);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterAmountFrom;
        public decimal? FilterAmountFrom
        {
            get => _filterAmountFrom;
            set
            {
                _filterAmountFrom = value;
                OnPropertyChanged(() => FilterAmountFrom);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterAmountTo;
        public decimal? FilterAmountTo
        {
            get => _filterAmountTo;
            set
            {
                _filterAmountTo = value;
                OnPropertyChanged(() => FilterAmountTo);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<ExpensesEntityForView> _filteredList;
        public ObservableCollection<ExpensesEntityForView> FilteredList
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
        public RealyCommand ApplyFiltersCommand { get; set; }
        public RealyCommand OpenAddNewExpenseCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewExpenseRequested;
        #endregion

        #region Constructor
        public ExpensesViewModel()
            : base()
        {
            _filterAmountTo = realEstateEntities.Expenses.Max(e => e.Amount);
            OpenAddNewExpenseCommand = new RealyCommand(o =>
            {
                AddNewExpenseRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from e in realEstateEntities.Expenses
                        join p in realEstateEntities.Projects on e.ProjectID equals p.ProjectID
                        select new ExpensesEntityForView
                        {
                            Id = e.ExpenseID,
                            ProjectName = p.ProjectName,
                            Address = p.Location,
                            ExpenseType = e.ExpenseType,
                            ExpenseAmount = e.Amount,
                            ExpenseDate = e.ExpenseDate
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ExpensesEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            var filtered = List.Where(e =>
                (string.IsNullOrEmpty(FilterProjectName) ||
                 (!string.IsNullOrEmpty(e.ProjectName) && e.ProjectName.Contains(FilterProjectName))) &&
                (string.IsNullOrEmpty(FilterExpenseType) ||
                 (!string.IsNullOrEmpty(e.ExpenseType) && e.ExpenseType.Contains(FilterExpenseType))) &&
                (!FilterAmountFrom.HasValue || e.ExpenseAmount >= FilterAmountFrom.Value) &&
                (!FilterAmountTo.HasValue || e.ExpenseAmount <= FilterAmountTo.Value)
            );

            FilteredList = new ObservableCollection<ExpensesEntityForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ExpensesEntityForView selected)
            {
                var modal = new DeleteExpenseModalView();
                DeleteExpenseModalViewModel deleteExpenseModalViewModel = new DeleteExpenseModalViewModel(
                                            realEstateEntities.Expenses.First(e => e.ExpenseID == SelectedItem.Id));
                deleteExpenseModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteExpenseModalViewModel;
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
