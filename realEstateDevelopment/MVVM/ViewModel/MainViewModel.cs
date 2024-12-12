using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Commands
        public RealyCommand HomeViewCommand { get; set; }
        public RealyCommand BuildingsViewCommand { get; set; }
        public RealyCommand ClientsViewCommand { get; set; }
        public RealyCommand SuppliersViewCommand { get; set; }
        public RealyCommand ProjectsViewCommand { get; set; }
        public RealyCommand ApartmentsViewCommand { get; set; }
        public RealyCommand ConstructionScheduleViewCommand { get; set; }
        public RealyCommand EmployeesViewCommand {  get; set; }
        public RealyCommand MaterialViewCommand { get; set; }
        public RealyCommand MaintenanceRequestsCommand {  get; set; }
        public RealyCommand ReservationRequestCommand { get; set; }
        public RealyCommand FinancesRequestCommand {  get; set; }
        public RealyCommand SalesRequestCommand { get; set; }
        public RealyCommand PurchasesRequestCommand { get; set; }
        public RealyCommand HistoryOfChangesViewCommand { get; set; }

        #endregion


        #region Properties
        public HomeViewModel HomeVM { get; set; }
        public BuildingsViewModel BuildingsVM { get; set; }
        public ClientViewModel ClientVM { get; set; }
        public SuppliersViewModel SuppliersVM { get; set; }
        public ProjectsViewModel ProjectsVM { get; set; }
        public ApartmentsViewModel ApartmentsVM { get; set; }
        public ConstructionScheduleViewModel ConstructionScheduleVM { get; set; }
        public EmployeesViewModel EmployeesVM { get; set; }
        public MaterialsViewModel MaterialsVM { get; set; }
        public MaintenanceRequestsViewModel MaintenanceRequestsVM {  get; set; }
        public ReservationsViewModel ReservationsVM { get; set; }
        public FinancesViewModel FinancesVM { get; set; }
        public SalesViewModel SalesVM { get; set; }
        public PurchasesViewModel PurchasesVM { get; set; }
        public HistoryOfChangesViewModel HistoryOfChangesVM { get; set; }
        
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

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            BuildingsVM = new BuildingsViewModel();
            ClientVM = new ClientViewModel();
            SuppliersVM = new SuppliersViewModel();
            ProjectsVM = new ProjectsViewModel();
            ApartmentsVM = new ApartmentsViewModel();
            ConstructionScheduleVM = new ConstructionScheduleViewModel();
            EmployeesVM = new EmployeesViewModel();
            MaterialsVM = new MaterialsViewModel();
            MaintenanceRequestsVM = new MaintenanceRequestsViewModel();
            ReservationsVM = new ReservationsViewModel();
            FinancesVM = new FinancesViewModel();
            SalesVM = new SalesViewModel();
            PurchasesVM = new PurchasesViewModel();
            HistoryOfChangesVM = new HistoryOfChangesViewModel();

            BuildingsVM.AddNewBuildingRequested += OnAddNewBuildingRequested;
            ClientVM.AddNewClientRequested += OnAddNewClientRequested;
            SuppliersVM.AddNewSupplierRequest += OnAddNewSupplierRequested;
            ProjectsVM.AddNewProjectRequested += OnAddNewProjectRequested;
            ApartmentsVM.AddNewApartmentRequested += OnAddNewApartmentRequested;
            ConstructionScheduleVM.ScheduleNewConstructionRequested += OnScheduleNewConstructionRequested;
            MaterialsVM.AddNewMaterialRequested += OnAddNewMaterialRequested;
            ReservationsVM.AddNewReservationRequested += OnAddNewReservationRequested;
            MaintenanceRequestsVM.AddNewMaintenanceRequestsRequested += OnAddNewMaintenanceRequestsRequested;
            EmployeesVM.AddNewEmployeeRequested += OnAddNewEmployeeRequested;



            CurrentView = HomeVM;

            HomeViewCommand = new RealyCommand(o =>
            {
                CurrentView = HomeVM;
            });

            BuildingsViewCommand = new RealyCommand(o =>
            {
                CurrentView = BuildingsVM;
            });

            ClientsViewCommand = new RealyCommand(o =>
            {
                CurrentView = ClientVM;
            });

            SuppliersViewCommand = new RealyCommand(o =>
            {
                CurrentView = SuppliersVM;
            });

            ProjectsViewCommand = new RealyCommand(o =>
            {
                CurrentView = ProjectsVM;
            });

            ApartmentsViewCommand = new RealyCommand(o =>
            {
                CurrentView = ApartmentsVM;
            });

            ConstructionScheduleViewCommand = new RealyCommand(o =>
            {
                CurrentView = ConstructionScheduleVM;
            });

            EmployeesViewCommand = new RealyCommand(o =>
            {
                CurrentView = EmployeesVM;
            });
            MaterialViewCommand = new RealyCommand(o =>
            {
                CurrentView = MaterialsVM;
            });
            MaintenanceRequestsCommand = new RealyCommand(o =>
            {
                CurrentView = MaintenanceRequestsVM;
            });
            ReservationRequestCommand = new RealyCommand(o =>
            {
                CurrentView = ReservationsVM;
            });
            FinancesRequestCommand = new RealyCommand(o =>
            {
                CurrentView = FinancesVM;
            });
            SalesRequestCommand = new RealyCommand(o =>
            {
                CurrentView = SalesVM;
            });
            PurchasesRequestCommand = new RealyCommand(o =>
            {
                CurrentView = PurchasesVM;
            });
            HistoryOfChangesViewCommand = new RealyCommand(o =>
            {
                CurrentView = HistoryOfChangesVM;
            });
        }

        #region Helpers
        private void OnAddNewBuildingRequested()
        {
            var addNewBuildingVM = new AddNewBuildingViewModel();
            addNewBuildingVM.RequestClose += (sender, args) =>
            {
                CurrentView = BuildingsVM;
            };
            CurrentView = addNewBuildingVM;
        }

        private void OnAddNewClientRequested()
        {
            var addNewClientVM = new AddNewClientViewModel();
            addNewClientVM.RequestClose += (sender, args) =>
            {
                CurrentView = ClientVM;
            };
            CurrentView = addNewClientVM;
        }

        private void OnAddNewSupplierRequested()
        {
            var addNewSupplierVM = new AddNewSupplierViewModel();
            addNewSupplierVM.RequestClose += (sender, args) =>
            {
                CurrentView = SuppliersVM;
            };
            CurrentView = addNewSupplierVM;
        }

        private void OnAddNewProjectRequested()
        {
            var addNewProjectVM = new AddNewProjectViewModel();
            addNewProjectVM.RequestClose += (sender, args) =>
            {
                CurrentView = ProjectsVM;
            };
            CurrentView = addNewProjectVM;
        }

        private void OnAddNewApartmentRequested()
        {
            var addNewApartmentVM = new AddNewApartmentViewModel();
            addNewApartmentVM.RequestClose += (sender, args) =>
            {
                CurrentView = ProjectsVM;
            };
            CurrentView = addNewApartmentVM;
        }


        private void OnScheduleNewConstructionRequested()
        {
            var scheduleBuildingConstructionVM = new ScheduleBuildingConstructionViewModel();
            scheduleBuildingConstructionVM.RequestClose += (sender, args) =>
            {
                CurrentView = ConstructionScheduleVM; 
            };
            CurrentView = scheduleBuildingConstructionVM;
        }

        private void OnAddNewMaterialRequested()
        {
            var addNewmaterialVM = new AddNewMaterialViewModel();
            addNewmaterialVM.RequestClose += (sender, args) =>
            {
                CurrentView = MaterialsVM; 
            };
            CurrentView = addNewmaterialVM;
        }

        private void OnAddNewReservationRequested()
        {
            var addNewReservationVM = new AddNewReservationsViewModel();
            addNewReservationVM.RequestClose += (sender, args) =>
            {
                CurrentView = ReservationsVM; 
            };
            CurrentView = addNewReservationVM;
        }

        private void OnAddNewMaintenanceRequestsRequested()
        {
            var addNewMaintenanceRequestVM = new AddNewMaintenanceRequestsViewModel();
            addNewMaintenanceRequestVM.RequestClose += (sender, args) =>
            {
                CurrentView = ReservationsVM;
            };
            CurrentView = addNewMaintenanceRequestVM;
        }

        private void OnAddNewEmployeeRequested()
        {
            var addNewEmployeeRequestVM = new AddNewEmployeeViewModel();
            addNewEmployeeRequestVM.RequestClose += (sender, args) =>
            {
                CurrentView = EmployeesVM;
            };
            CurrentView = addNewEmployeeRequestVM;
        }
        #endregion
    }
}
