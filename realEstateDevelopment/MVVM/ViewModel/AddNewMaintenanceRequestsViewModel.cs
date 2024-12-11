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
        private string _selectedApartment;
        private string _selectedClient;
        public ObservableCollection<string> AvailableApartments { get; set; }
        public ObservableCollection<string> AllClients { get; set; }

        public string SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(() => SelectedClient);
            }
        }

        public string SelectedApartment
        {
            get => _selectedApartment;
            set
            {
                _selectedApartment = value;
                OnPropertyChanged(() => SelectedApartment);
            }
        }

        public int ClientID
        {
            get
            {
                return item.ClientID;
            }
            set
            {
                var text = SelectedClient.Replace("ID:", "").Trim(); // Usuwamy "ID:" i nadmiarowe spacje
                Console.WriteLine(text);
                item.ClientID = Convert.ToInt32(text.Split(' ')[0]); // Teraz bierzemy tylko pierwszą część (ID)
                OnPropertyChanged(() => ClientID);
            }
        }

        public int ApartmentID
        {
            get
            {
                return item.ApartmentID;
            }
            set
            {
                var text = SelectedApartment.Replace("ID:", "").Trim();
                item.ApartmentID = Convert.ToInt32(text.Split(' ')[0]);
                OnPropertyChanged(() => ApartmentID);
            }
        }

        public DateTime RequestDate
        {
            get
            {
                return (DateTime)item.RequestDate;

            }
            set
            {
                item.RequestDate = DateTime.Now;
                OnPropertyChanged(() => RequestDate);
            }
        }

        public string Description
        {
            get
            {
                return item.Description;
            }
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        #region Constructor

        public AddNewMaintenanceRequestsViewModel()
        {
            //dodawanie wartości domyślnych i tworzenie ObservableCollection do obsługi wyświetlania napisów w dropdown
            AllClients = new ObservableCollection<string> { "Wybierz klienta" };
            AvailableApartments = new ObservableCollection<string> { "Wybierz mieszkanie" };

            var query = from a in estateEntities.Apartments
                        where (a.Status != "Zarezerwowano" && a.Status != "Wynajęto")
                        join b in estateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in estateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new AddNewMaintenanceRequestsEntityForView
                        {
                            ApartmentId = a.ApartmentID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                        };

            foreach (var entity in query)
            {
                AvailableApartments.Add($"ID: {entity.ApartmentId} Mieszkanie: {entity.ApartmentNumber} Budynek: {entity.BuildingNumber} Adres: {entity.Address}");
            }



            var query2 = from c in estateEntities.Clients
                         select new AddNewMaintenanceRequestsEntityForView
                         {
                             ClientId = c.ClientID,
                             ClientName = c.FirstName,
                             ClientLastname = c.LastName,
                             ClientPesel = c.Pesel,
                         };
            foreach (var entity in query2)
            {
                AllClients.Add($"ID: {entity.ClientId} Imię: {entity.ClientName} Nazwisko: {entity.ClientLastname} Pesel: {entity.ClientPesel}");
            }

            // Ustawienie domyślnych wartości
            SelectedClient = AllClients.First();
            SelectedApartment = AvailableApartments.First();

            item = new MaintenanceRequests();
        }

        #endregion

        #region Validation
        public void Validate()
        {
            Console.WriteLine(item.ApartmentID);
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine("***");
            }

            Console.WriteLine("Client ID" + item.ClientID);
            Console.WriteLine("Apartment ID" + item.ApartmentID);
            isDataCorrect = true;
            var errors = new List<string>();


            if (SelectedClient.Equals("Wybierz klienta"))
            {
                errors.Add("Nie wybrano klienta.");
                isDataCorrect = false;
            }
            if (SelectedApartment.Equals("Wybierz mieszkanie"))
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
                
                estateEntities.MaintenanceRequests.Add(item);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }
        #endregion

    }

}