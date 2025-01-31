using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateMaintenanceRequestModalViewModel : BaseDataUpdater<MaintenanceRequests>
    {
        #region Properties


        private ApartmentForView _selectedApartment;


        public ObservableCollection<ApartmentForView> AvailableApartments { get; set; }

        public ApartmentForView SelectedApartment
        {
            get
            {
                return _selectedApartment;
            }

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

        private ClientForView _selectedClient;


        public ObservableCollection<ClientForView> AvailableClients { get; set; }

        public ClientForView SelectedClient
        {
            get
            {
                return _selectedClient;
            }

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

        

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        public DateTime RequestDate
        {
            get => (DateTime)item.RequestDate;
            set
            {
                item.RequestDate = value;
                OnPropertyChanged(() => RequestDate);
            }
        }

        public int ClientID
        {
            get => item.ClientID;
            set
            {
                item.ClientID = value;
                OnPropertyChanged(() => ClientID);
            }
        }

        public int ApartmentId
        {
            get => item.ApartmentID;
            set
            {
                item.ApartmentID = value;
                OnPropertyChanged(() => ApartmentId);
            }
        }

        #endregion

        #region Constructor

        public UpdateMaintenanceRequestModalViewModel(MaintenanceRequests maintenanceRequest)
            : base()
        {
            item = maintenanceRequest;
            AvailableClients = new ObservableCollection<ClientForView>();
            AvailableApartments = new ObservableCollection<ApartmentForView>();
            LoadClients();
            LoadApartments();
            SelectedClient = AvailableClients.FirstOrDefault(c => c.Id == ClientID);
            SelectedApartment = AvailableApartments.FirstOrDefault(a=> a.Id == ApartmentId);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (ApartmentId <= 0)
            {
                errors.Add("ID mieszkania nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (ClientID <= 0)
            {
                errors.Add("ID klienta nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                errors.Add("Opis zgłoszenia jest wymagany.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status zgłoszenia jest wymagany.");
                isDataCorrect = false;
            }

            if (RequestDate == default(DateTime))
            {
                errors.Add("Data zgłoszenia jest wymagana.");
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
                var existingItem = estateEntities.MaintenanceRequests.FirstOrDefault(m => m.RequestID == item.RequestID);
                if (existingItem != null)
                {
                    existingItem.ApartmentID = item.ApartmentID;
                    existingItem.ClientID = item.ClientID;
                    existingItem.Description = item.Description;
                    existingItem.Status = item.Status;
                    existingItem.RequestDate = item.RequestDate;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono zgłoszenia do aktualizacji.");
                    errorModal.ShowDialog();
                }
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }


        public void ShowMessageBox(string message)
        {
            var updateMaintenanceRequestModal = new UpdateMaintenanceRequestModalView();
            updateMaintenanceRequestModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateMaintenanceRequestModal.Close();
            };

            updateMaintenanceRequestModal.ShowDialog();
        }

        private void LoadClients()
        {
            var clients = (from c in estateEntities.Clients
                             select new ClientForView
                             {
                                 Id = c.ClientID,
                                 Name = c.FirstName,
                                 Surname = c.LastName,
                                 Pesel = c.Pesel,
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
