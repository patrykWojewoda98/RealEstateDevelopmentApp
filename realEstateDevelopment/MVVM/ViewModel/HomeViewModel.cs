using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private RealEstateEntities estateEntities;

        private string _welcome;
        private ObservableCollection<string> _projects;
        private string _selectedProject;
        private int _numberOfBuildings;
        private ObservableCollection<string> _maintenanceRequests;
        private ObservableCollection<string> _recentChanges;

        public string Welcome
        {
            get => _welcome;
            set
            {
                _welcome = value;
                OnPropertyChanged(nameof(Welcome));
            }
        }

        public ObservableCollection<string> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public string SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                LoadProjectData();
            }
        }

        public int NumberOfBuildings
        {
            get => _numberOfBuildings;
            set
            {
                _numberOfBuildings = value;
                OnPropertyChanged(nameof(NumberOfBuildings));
            }
        }

        public ObservableCollection<string> MaintenanceRequests
        {
            get => _maintenanceRequests;
            set
            {
                _maintenanceRequests = value;
                OnPropertyChanged(nameof(MaintenanceRequests));
            }
        }

        public ObservableCollection<string> RecentChanges
        {
            get => _recentChanges;
            set
            {
                _recentChanges = value;
                OnPropertyChanged(nameof(RecentChanges));
            }
        }

        public HomeViewModel()
        {
            estateEntities = new RealEstateEntities();
            Projects = new ObservableCollection<string>();
            MaintenanceRequests = new ObservableCollection<string>();
            RecentChanges = new ObservableCollection<string>();

            if (MainViewModel.employee != null)
            {
                Welcome = $"Witaj {MainViewModel.employee.FirstName} {MainViewModel.employee.LastName}!";
            }
            else
            {
                Welcome = "Witaj użytkowniku!";
            }

            // Pobranie wszystkich dostępnych projektów
            var allProjects = estateEntities.Projects
                .Select(p => p.ProjectName)
                .ToList();

            if (allProjects.Any())
            {
                foreach (var project in allProjects)
                {
                    Projects.Add(project);
                }
                SelectedProject = Projects.FirstOrDefault();
            }
            else
            {
                Projects.Add("Brak dostępnych projektów");
                SelectedProject = "Brak dostępnych projektów";
            }
        }

        private void LoadProjectData()
        {
            var selectedProjectEntity = estateEntities.Projects.FirstOrDefault(p => p.ProjectName == SelectedProject);
            if (selectedProjectEntity != null)
            {
                NumberOfBuildings = estateEntities.Buildings.Count(b => b.ProjectID == selectedProjectEntity.ProjectID);

                // Pobierz listę zgłoszeń konserwacyjnych, które nie są "Zrealizowano"
                var maintenanceRequestsData = estateEntities.MaintenanceRequests
                    .Where(mr => mr.Status != "Zrealizowano" &&
                        estateEntities.Apartments.Any(a => a.BuildingID ==
                            estateEntities.Buildings.FirstOrDefault(b => b.ProjectID == selectedProjectEntity.ProjectID).BuildingID
                            && a.ApartmentID == mr.ApartmentID))
                    .OrderByDescending(mr => mr.RequestDate)
                    .ToList();

                MaintenanceRequests.Clear();
                foreach (var request in maintenanceRequestsData)
                {
                    MaintenanceRequests.Add(FormatMaintenanceRequest(request));
                }

                // Pobierz 3 ostatnie zmiany w historii zmian
                var recentChangesList = estateEntities.HistoryOfChanges
                    .OrderByDescending(h => h.DateAndTimeOfChange)
                    .Take(3)
                    .AsEnumerable()
                    .Select(h => $"{MapOperationName(h.Operation)} ({h.DateAndTimeOfChange:dd-MM-yyyy HH:mm})")
                    .ToList();

                RecentChanges.Clear();
                foreach (var change in recentChangesList)
                {
                    RecentChanges.Add(change);
                }
            }
        }

        private string FormatMaintenanceRequest(MaintenanceRequests request)
        {
            var apartment = estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == request.ApartmentID);
            var building = estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == apartment.BuildingID);
            var project = estateEntities.Projects.FirstOrDefault(p => p.ProjectID == building.ProjectID);

            return $"{request.Description} - Budynek: {building.BuildingNumber}, Adres: {project.Location}, Data: {request.RequestDate:dd-MM-yyyy}";
        }

        private string MapOperationName(string operation)
        {
            if (string.IsNullOrEmpty(operation))
                return "Nieznana operacja";

            switch (operation)
            {
                case "AddNewApartmentViewModel": return "Dodanie nowego apartamentu";
                case "AddNewBuildingViewModel": return "Dodanie nowego budynku";
                case "AddNewClientViewModel": return "Dodanie nowego klienta";
                case "AddNewEmployeeViewModel": return "Dodanie nowego pracownika";
                case "AddNewExpenseViewModel": return "Dodanie nowego wydatku";
                case "UpdateApartmentModalViewModel": return "Aktualizacja apartamentu";
                case "UpdateBuildingModalViewModel": return "Aktualizacja budynku";
                case "DeleteApartmentModalViewModel": return "Usunięcie apartamentu";
                case "DeleteBuildingModalViewModel": return "Usunięcie budynku";
                case "DeleteClientModalViewModel": return "Usunięcie klienta";
                case "DeleteEmployeeModalViewModel": return "Usunięcie pracownika";
                default: return operation;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
