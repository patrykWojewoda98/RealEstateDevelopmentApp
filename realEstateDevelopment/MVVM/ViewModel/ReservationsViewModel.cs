using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ReservationsViewModel : LoadAllViewModel<ReservationsEntityForView>
    {
        #region Properties
        private ReservationsEntityForView _selectedItem;
        public ReservationsEntityForView SelectedItem
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
        public RealyCommand OpenAddNewReservationCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewReservationRequested;
        #endregion

        #region Constructor
        public ReservationsViewModel()
            :base()
        {
            OpenAddNewReservationCommand = new RealyCommand(o =>
            {
                AddNewReservationRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from r in realEstateEntities.Reservations
                        join c in realEstateEntities.Clients on r.ClientID equals c.ClientID
                        join a in realEstateEntities.Apartments on r.ApartmentID equals a.ApartmentID
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new ReservationsEntityForView
                        {
                            Id = r.ReservationID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            ReservationDate = r.ReservationDate,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            ClientPhoneNumber = c.PhoneNumber,
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ReservationsEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ReservationsEntityForView selected)
            {
                var modal = new DeleteReservationModalView();
                DeleteReservationModalViewModel deleteReservationModalViewModel = new DeleteReservationModalViewModel(
                                            realEstateEntities.Reservations.First(r => r.ReservationID == SelectedItem.Id));
                deleteReservationModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteReservationModalViewModel;
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
