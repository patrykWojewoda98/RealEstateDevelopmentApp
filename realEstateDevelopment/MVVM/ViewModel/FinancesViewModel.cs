using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;


namespace realEstateDevelopment.MVVM.ViewModel
{
    public class FinancesViewModel : ObservableObject
    {
        #region Commands
        public RealyCommand RevenueViewCommand { get; set; }
        public RealyCommand ExpenseViewCommand { get; set; }
        public RealyCommand AllCashOperationsViewCommand { get; set; }  

        #endregion


        #region Properties
        public RevenuesViewModel RevenuesVM { get; set; }
        public ExpensesViewModel ExpensesVM { get; set; }
        public AllCashOperationsViewModel AllCashOperationsVM { get; set; }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public FinancesViewModel()
        {
            RevenuesVM = new RevenuesViewModel();
            ExpensesVM = new ExpensesViewModel();
            AllCashOperationsVM = new AllCashOperationsViewModel();
            


            RevenueViewCommand = new RealyCommand(o =>
            {
                CurrentView = RevenuesVM;
            });

            ExpenseViewCommand = new RealyCommand(o =>
            {
                CurrentView = ExpensesVM;
            });

            AllCashOperationsViewCommand = new RealyCommand(o =>
            {
                CurrentView = AllCashOperationsVM;
            });

            RevenuesVM.AddNewRevenueRequested += OnAddNewRevenueRequested;
            ExpensesVM.AddNewExpenseRequested += OnAddNewExpenseRequested;


        }
        #endregion

        #region Helpers
        private void OnAddNewRevenueRequested()
        {
            var addNewRevenueVM = new AddNewRevenueViewModel();
            addNewRevenueVM.RequestClose += (sender, args) =>
            {
                CurrentView = RevenuesVM;
            };
            CurrentView = addNewRevenueVM;
        }
        
        private void OnAddNewExpenseRequested()
        {
            var addNewExpenseVM = new AddNewExpenseViewModel();
            addNewExpenseVM.RequestClose += (sender, args) =>
            {
                CurrentView = ExpensesVM;
            };
            CurrentView = addNewExpenseVM;
        }
        #endregion
    }
}
