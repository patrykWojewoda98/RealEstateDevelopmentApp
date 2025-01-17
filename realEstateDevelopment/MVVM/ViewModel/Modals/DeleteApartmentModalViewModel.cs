using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
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
            var deleteApartmentModal = new DeleteApartmentModalView(); // To powinien być Twój UserControl/Window
            deleteApartmentModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                deleteApartmentModal.Close();
            };

            deleteApartmentModal.ShowDialog(); // Jeśli to jest Window
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

