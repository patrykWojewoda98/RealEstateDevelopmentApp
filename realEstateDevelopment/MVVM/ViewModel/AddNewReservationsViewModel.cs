using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewReservationsViewModel : BaseDatabaseAdder<Reservations>
    {
        private ApartmentForView _selectedApartment;
        private ClientForView _selectedClient;

        public ObservableCollection<ApartmentForView> AvailableApartments { get; set; }
        public ObservableCollection<ClientForView> AllClients { get; set; }

        public ClientForView SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                if (_selectedClient != null)
                {
                    item.ClientID = _selectedClient.Id;
                }
                OnPropertyChanged(() => SelectedClient);
            }
        }

        public ApartmentForView SelectedApartment
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

        public DateTime ReservationDate
        {
            get => item.ReservationDate;
            set
            {
                item.ReservationDate = DateTime.Now;
                OnPropertyChanged(() => ReservationDate);
            }
        }

        #region Constructor
        public AddNewReservationsViewModel()
        {
            AllClients = new ObservableCollection<ClientForView>();
            AvailableApartments = new ObservableCollection<ApartmentForView>();

            LoadClients();
            LoadApartments();

            SelectedClient = null;
            SelectedApartment = null;

            item = new Reservations()
            {
                ReservationDate = DateTime.Now,
            };
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            if (SelectedClient == null)
            {
                errors.Add("Nie wybrano klienta.");
                isDataCorrect = false;
            }
            if (SelectedApartment == null)
            {
                errors.Add("Nie wybrano mieszkania.");
                isDataCorrect = false;
            }
            potentialErrors = string.Join(Environment.NewLine, errors);
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            Validate();
            if (isDataCorrect)
            {
                var apartment = estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID);
                if (apartment != null)
                {
                    apartment.Status = "Zarezerwowano";
                    estateEntities.SaveChanges();
                }
                estateEntities.Reservations.Add(item);
                estateEntities.SaveChanges();
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
                Pesel = c.Pesel
            }).ToList();

            foreach (var client in clients)
            {
                AllClients.Add(client);
            }
        }

        private void LoadApartments()
        {
            var apartments = (from a in estateEntities.Apartments
                              where a.Status != "Zarezerwowano" && a.Status != "Wynajęto"
                              join b in estateEntities.Buildings on a.BuildingID equals b.BuildingID
                              join p in estateEntities.Projects on b.ProjectID equals p.ProjectID
                              select new ApartmentForView
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

        #endregion
    }
}
