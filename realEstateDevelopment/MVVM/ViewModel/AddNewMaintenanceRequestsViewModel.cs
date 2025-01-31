using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewMaintenanceRequestsViewModel : BaseDatabaseAdder<MaintenanceRequests>
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

                    // Załaduj dostępne mieszkania dla wybranego klienta
                    LoadApartmentsForClient(_selectedClient.Id);
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

        public DateTime RequestDate
        {
            get => item.RequestDate ?? DateTime.Now;
            set
            {
                item.RequestDate = value;
                OnPropertyChanged(() => RequestDate);
            }
        }

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        #region Constructor

        public AddNewMaintenanceRequestsViewModel()
        {
            item = new MaintenanceRequests();

            AllClients = new ObservableCollection<ClientForView>();
            AvailableApartments = new ObservableCollection<ApartmentForView>();
            LoadClients();

            SelectedClient = AllClients.FirstOrDefault();
            SelectedApartment = AvailableApartments.FirstOrDefault();

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

            if (string.IsNullOrWhiteSpace(Description))
            {
                errors.Add("Opis zgłoszenia nie może być pusty.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Description):
                    return string.IsNullOrWhiteSpace(Description) ? "Opis zgłoszenia nie może być pusty." : string.Empty;

                default:
                    return string.Empty;
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            Validate();
            if (isDataCorrect)
            {
                item.Status = "Do realizacji";
                item.RequestDate = DateTime.Now;
                SaveHistoryOfChanges();
                estateEntities.MaintenanceRequests.Add(item);
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

        


        private void LoadApartmentsForClient(int clientId)
        {
            AvailableApartments.Clear();
            var apartments = (from a in estateEntities.Apartments
                              where (a.ClientID == clientId && a.Status == "Zarezerwowano") || (a.ClientID == clientId && a.Status == "Wynajęto")
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

            // Reset SelectedApartment, jeśli lista została zmieniona
            SelectedApartment = AvailableApartments.FirstOrDefault();
        }
        #endregion
    }
}
