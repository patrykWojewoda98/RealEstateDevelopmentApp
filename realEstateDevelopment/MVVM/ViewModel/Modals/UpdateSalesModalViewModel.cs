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
    public class UpdateSalesModalViewModel : BaseDataUpdater<Sales>
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

        public int SaleId
        {
            get => item.SaleID;
            set
            {
                item.SaleID = value;
                OnPropertyChanged(() => SaleId);
            }
        }

        public decimal Amount
        {
            get => item.SalePrice;
            set
            {
                item.SalePrice = value;
                OnPropertyChanged(() => Amount);
            }
        }

        public DateTime SaleDate
        {
            get => item.SaleDate;
            set
            {
                item.SaleDate = value;
                OnPropertyChanged(() => SaleDate);
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

        

        #endregion

        #region Constructor

        public UpdateSalesModalViewModel(Sales sale)
            : base()
        {
            item = sale;
            AvailableApartments = new ObservableCollection<ApartmentForView>();
            AvailableClients = new ObservableCollection<ClientForView>();   
            LoadApartments();
            LoadClients();

            SelectedApartment = AvailableApartments.FirstOrDefault(a => a.Id == item.ApartmentID);
            SelectedClient = AvailableClients.FirstOrDefault(c => c.Id == item.ClientID);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (SelectedClient == null || SelectedClient.Id <= 0)
            {
                errors.Add("Musisz wybrać klienta.");
                isDataCorrect = false;
            }

            if (SelectedApartment == null || SelectedApartment.Id <= 0)
            {
                errors.Add("Musisz wybrać mieszkanie.");
                isDataCorrect = false;
            }

            if (Amount <= 0)
            {
                errors.Add("Cena sprzedaży musi być większa od 0.");
                isDataCorrect = false;
            }

            if (SaleDate == default)
            {
                errors.Add("Data sprzedaży jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status jest wymagany.");
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
                var existingItem = estateEntities.Sales.FirstOrDefault(s => s.SaleID == item.SaleID);

                if (existingItem != null)
                {
                    existingItem.ClientID = item.ClientID;
                    existingItem.ApartmentID = item.ApartmentID;
                    existingItem.SalePrice = item.SalePrice;
                    existingItem.SaleDate = item.SaleDate;
                    existingItem.Status = item.Status;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono sprzedaży do aktualizacji.");
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
            var updateSaleModal = new UpdateSalesModalView();
            updateSaleModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateSaleModal.Close();
            };

            updateSaleModal.ShowDialog();
        }

        private void LoadApartments()
        {
            var apartments = (from a in estateEntities.Apartments
                             select new ApartmentForView
                             {
                                 Id = a.ApartmentID,
                                 ApartmentNumber = a.ApartmentNumber,
                                 BuildingNumber = estateEntities.Buildings.FirstOrDefault(b=> b.BuildingID == a.BuildingID).BuildingNumber,
                                 Address = estateEntities.Projects.FirstOrDefault(p => p.ProjectID == estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == a.BuildingID).ProjectID).Location,
                             }).ToList();

            foreach (var apartment in apartments)
            {
                AvailableApartments.Add(apartment);
            }
        }

        private void LoadClients()
        {
            var clients = (from c in estateEntities.Clients
                              select new ClientForView
                              {
                                  Id= c.ClientID,
                                  Name = c.FirstName,
                                  Surname = c.LastName,
                                  Pesel = c.Pesel,
                              }).ToList();

            foreach (var client in clients)
            {
                AvailableClients.Add(client);
            }
        }

        #endregion
    }
}