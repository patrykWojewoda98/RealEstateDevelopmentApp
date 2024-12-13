using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class FinancesViewModel : ObservableObject
    {
        #region Commands
        public RealyCommand RevenueViewCommand { get; set; }
        public RealyCommand ExpenseViewCommand { get; set; }

        #endregion


        #region Properties
        public RevenuesViewModel RevenuesVM { get; set; }
        public ExpensesViewModel ExpensesVM { get; set; }
        

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
            

            RevenueViewCommand = new RealyCommand(o =>
            {
                CurrentView = RevenuesVM;
            });

            ExpenseViewCommand = new RealyCommand(o =>
            {
                CurrentView = ExpensesVM;
            });

            RevenuesVM.AddNewRevenueRequested += OnAddNewBuildingRequested;



        }
        #endregion

        #region Helpers
        private void OnAddNewBuildingRequested()
        {
            var addNewRevenueVM = new AddNewRevenueViewModel();
            addNewRevenueVM.RequestClose += (sender, args) =>
            {
                CurrentView = RevenuesVM;
            };
            CurrentView = addNewRevenueVM;
        }
        #endregion

    }
}
