using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteApartmentModalViewModel : BaseDataDeleter<Apartments>
    {
        #region Propeties

        private RealEstateEntities estateEntities;
        public int ApartmentID
        {
            get
            {
                return item.ApartmentID;
            }
            set
            {
                item.ApartmentID = value;
                OnPropertyChanged(() => ApartmentID);
            }
        }

        public int BuildingID
        {
            get
            {
                return item.BuildingID;
            }
            set
            {
                item.BuildingID = value;
                OnPropertyChanged(() => BuildingID);
            }
        }

        public string BuildingName
        {
            get
            {
                return estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == BuildingID).BuildingName;
            }
        }

        public string BuildingNumber
        {
            get
            {
                return estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == BuildingID).BuildingNumber;
            }
            
        }

        public string ApartmentNumber
        {
            get
            {
                return item.ApartmentNumber;
            }
            set
            {
                item.ApartmentNumber = value;
                OnPropertyChanged(() => ApartmentNumber);
            }
        }
        public decimal Area
        {
            get
            {
                return item.Area;
            }
            set
            {
                item.Area = value;
                OnPropertyChanged(() => Area);
            }
        }

        public int RoomCount
        {
            get
            {
                return item.RoomCount;
            }
            set
            {
                item.RoomCount = value;
                OnPropertyChanged(() => RoomCount);
            }
        }

        public int Floor
        {
            get
            {
                return item.Floor;
            }
            set
            {
                item.Floor = value;
                OnPropertyChanged(() => Floor);
            }
        }

        public string Status
        {
            get
            {
                return item.Status;
            }
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        public string Address
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == estateEntities.Buildings.FirstOrDefault(b => b.ProjectID == p.ProjectID).ProjectID).Location;
        }
        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        #endregion 

        #region Constructor
        public DeleteApartmentModalViewModel(Apartments apartment)
            : base()
        {
            item = apartment;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            
        }
        #endregion

        


        #region Helpers
        public override void Delete()
        {
             var existingItem = estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID);
             if (existingItem != null)
             {

                    // Usuń sprzedaż powiązaną z mieszkaniem
                    var salesToRemove = estateEntities.Sales
                        .Where(s => s.ApartmentID == existingItem.ApartmentID)
                        .ToList();
                    foreach (var sale in salesToRemove)
                    {
                        estateEntities.Sales.Remove(sale);
                    }

                    // Usuń zgłoszenia konserwacyjne powiązane z mieszkaniem
                    var maintenanceRequestsToRemove = estateEntities.MaintenanceRequests
                        .Where(m => m.ApartmentID == existingItem.ApartmentID)
                        .ToList();
                    foreach (var maintenanceRequest in maintenanceRequestsToRemove)
                    {
                        estateEntities.MaintenanceRequests.Remove(maintenanceRequest);
                    }

                    // Usuń wynajem powiązany z mieszkaniem
                    var rentalsToRemove = estateEntities.Rental
                        .Where(r => r.ApartmentId == existingItem.ApartmentID)
                        .ToList();
                    foreach (var rental in rentalsToRemove)
                    {
                        estateEntities.Rental.Remove(rental);
                    }
                


                estateEntities.Apartments.Remove(existingItem);

                 estateEntities.SaveChanges();
             }
             else
             {
                 var errorModal = new ErrorModalView("Nie znaleziono elementu do aktualizacji.");
                 errorModal.ShowDialog();
             }
        }

        public void ShowMessageBox(string message)
        {
            // Zamiast wyświetlać alert, otwórz modal.
            var deleteApartmentModal = new DeleteApartmentModalView();
            deleteApartmentModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                deleteApartmentModal.Close();
            };

            deleteApartmentModal.ShowDialog();
        }

        private void ExecuteDelete(object parameter)
        {
            try
            {
                Delete(); // Wywołanie istniejącej metody Delete
                base.OnRequestClose();
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                var errorModal = new ErrorModalView($"Wystąpił błąd: {ex.Message}");
                errorModal.ShowDialog();
            }
        }
        #endregion
    }
}

