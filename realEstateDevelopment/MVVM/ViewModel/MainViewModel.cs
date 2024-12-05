using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;

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
        private object _currentView;
        #endregion

        
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

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

            BuildingsVM.AddNewBuildingRequested += OnAddNewBuildingRequested;
            ClientVM.AddNewClientRequested += OnAddNewClientRequested;
            SuppliersVM.AddNewSupplierRequest += OnAddNewSupplierRequested;
            ProjectsVM.AddNewProjectRequested += OnAddNewProjectRequested;
            ApartmentsVM.AddNewApartmentRequested += OnAddNewApartmentRequested;
            




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
        #endregion
    }
}
