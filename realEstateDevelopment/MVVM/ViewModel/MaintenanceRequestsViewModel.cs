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
    public class MaintenanceRequestsViewModel : LoadAllViewModel<MaintenanceRequestsEntityForView>
    {
        #region Properties
        private MaintenanceRequestsEntityForView _selectedItem;
        public MaintenanceRequestsEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private DateTime? _filterRequestDateFrom;
        public DateTime? FilterRequestDateFrom
        {
            get => _filterRequestDateFrom;
            set
            {
                _filterRequestDateFrom = value;
                OnPropertyChanged(() => FilterRequestDateFrom);
                ApplyFiltersAsync();
            }
        }

        private DateTime? _filterRequestDateTo;
        public DateTime? FilterRequestDateTo
        {
            get => _filterRequestDateTo;
            set
            {
                _filterRequestDateTo = value;
                OnPropertyChanged(() => FilterRequestDateTo);
                ApplyFiltersAsync();
            }
        }

        private string _filterClientName;
        public string FilterClientName
        {
            get => _filterClientName;
            set
            {
                _filterClientName = value;
                OnPropertyChanged(() => FilterClientName);
                ApplyFiltersAsync();
            }
        }

        private string _filterStatus;
        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(() => FilterStatus);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<MaintenanceRequestsEntityForView> _filteredList;
        public ObservableCollection<MaintenanceRequestsEntityForView> FilteredList
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
        public RealyCommand OpenAddNewMaintenanceRequestsCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewMaintenanceRequestsRequested;
        #endregion

        #region
        public MaintenanceRequestsViewModel()
            :base()
        {
            OpenAddNewMaintenanceRequestsCommand = new RealyCommand(o =>
            {
                AddNewMaintenanceRequestsRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion
        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from m in realEstateEntities.MaintenanceRequests
                        join c in realEstateEntities.Clients on m.ClientID equals c.ClientID
                        join a in realEstateEntities.Apartments on m.ApartmentID equals a.ApartmentID
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new MaintenanceRequestsEntityForView
                        {
                            Id = m.RequestID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            Description = m.Description,
                            RequestDate = m.RequestDate ?? default,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            ClientPhoneNumber = c.PhoneNumber,
                            Status = m.Status
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<MaintenanceRequestsEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<MaintenanceRequestsEntityForView>(
                List.Where(m =>
                    (!FilterRequestDateFrom.HasValue || m.RequestDate >= FilterRequestDateFrom) &&
                    (!FilterRequestDateTo.HasValue || m.RequestDate <= FilterRequestDateTo) &&
                    (string.IsNullOrEmpty(FilterClientName) || m.ClientName.Contains(FilterClientName)) &&
                    (string.IsNullOrEmpty(FilterStatus) || m.Status.Contains(FilterStatus))
                ));
            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is MaintenanceRequestsEntityForView selected)
            {
                var modal = new DeleteMaintenanceRequestModalView();
                DeleteMaintenanceRequestModalViewModel deleteMaintenanceRequestModalViewModel = new DeleteMaintenanceRequestModalViewModel(
                                            realEstateEntities.MaintenanceRequests.First(m => m.RequestID == SelectedItem.Id));
                deleteMaintenanceRequestModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteMaintenanceRequestModalViewModel;
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
