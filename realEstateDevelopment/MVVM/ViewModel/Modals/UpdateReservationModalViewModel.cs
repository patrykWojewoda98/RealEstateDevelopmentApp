using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateReservationModalViewModel : BaseDataUpdater<Reservations>
    {
        #region Properties

        public int ReservationId { get; set; }

        private ClientForView _selectedClient;
        private ReservationsEntityForView _selectedApartment;

        public ObservableCollection<ClientForView> AvailableClients { get; set; }
        public ObservableCollection<ReservationsEntityForView> AvailableApartments { get; set; }

        public ClientForView SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(() => SelectedClient);
            }
        }

        public ReservationsEntityForView SelectedApartment
        {
            get => _selectedApartment;
            set
            {
                _selectedApartment = value;
                if (_selectedApartment != null)
                {
                    item.ApartmentID = _selectedApartment.Id;
                }
                OnPropertyChanged(() => SelectedApartment);
            }
        }

        public int ReservationID
        {
            get => item.ReservationID;
            set
            {
                item.ReservationID = value;
                OnPropertyChanged(() => ReservationID);
            }
        }

        public DateTime ReservationDate
        {
            get => item.ReservationDate;
            set
            {
                item.ReservationDate = value;
                OnPropertyChanged(() => ReservationDate);
            }
        }

        #endregion

        #region Constructor

        public UpdateReservationModalViewModel(Reservations reservation)
            : base()
        {
            item = reservation;

            AvailableClients = new ObservableCollection<ClientForView>();
            AvailableApartments = new ObservableCollection<ReservationsEntityForView>();

            LoadClients();
            LoadApartments();

            SelectedClient = AvailableClients.FirstOrDefault(c => c.Id == reservation.ClientID);
            SelectedApartment = AvailableApartments.FirstOrDefault(a => a.Id == reservation.ApartmentID);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (ReservationID <= 0)
            {
                errors.Add("ID rezerwacji nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (SelectedClient == null)
            {
                errors.Add("Musisz wybrać klienta.");
                isDataCorrect = false;
            }

            if (SelectedApartment == null)
            {
                errors.Add("Musisz wybrać apartament.");
                isDataCorrect = false;
            }

            if (ReservationDate == default)
            {
                errors.Add("Data rezerwacji jest wymagana.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }

        #endregion

        #region Helpers

        public override void Update()
        {
            Validate();
            if (isDataCorrect)
            {
                var existingItem = estateEntities.Reservations.FirstOrDefault(r => r.ReservationID == ReservationID);
                if (existingItem != null)
                {
                    existingItem.ClientID = item.ClientID;
                    existingItem.ApartmentID = item.ApartmentID;
                    existingItem.ReservationDate = item.ReservationDate;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono rezerwacji do aktualizacji.");
                    errorModal.ShowDialog();
                }
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }

        private void LoadClients()
        {
            var clients = estateEntities.Clients.Select(c => new ClientForView
            {
                Id = c.ClientID,
                Name = c.FirstName,
                Surname = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email
            }).ToList();

            foreach (var client in clients)
            {
                AvailableClients.Add(client);
            }
        }

        private void LoadApartments()
        {
            var apartments = (from a in estateEntities.Apartments
                              join b in estateEntities.Buildings on a.BuildingID equals b.BuildingID
                              join p in estateEntities.Projects on b.ProjectID equals p.ProjectID
                              select new ReservationsEntityForView
                              {
                                  Id = a.ApartmentID,
                                  ApartmentNumber = a.ApartmentNumber,
                                  BuildingNumber = b.BuildingNumber,
                                  Address = p.Location
                              }).ToList();

            foreach (var apartment in apartments)
            {
                AvailableApartments.Add(apartment);
            }
        }

        public void ShowMessageBox(string message)
        {
            var updateReservationModal = new UpdateReservationModalView();
            updateReservationModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateReservationModal.Close();
            };

            updateReservationModal.ShowDialog();
        }

        #endregion
    }
}
