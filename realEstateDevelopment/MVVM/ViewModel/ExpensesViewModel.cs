using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
        #endregion
        #region Commands
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
            OpenAddNewExpenseCommand = new RealyCommand(o =>
            {
                AddNewExpenseRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
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
            throw new NotImplementedException();
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
