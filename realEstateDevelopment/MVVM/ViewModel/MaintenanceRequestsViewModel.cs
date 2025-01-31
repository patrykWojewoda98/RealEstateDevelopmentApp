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
        #endregion
        #region Commands
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
            throw new NotImplementedException();
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
