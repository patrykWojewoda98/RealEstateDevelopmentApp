using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateApartmentModalViewModel : BaseDataUpdater<Apartments>
    {
        #region Propeties

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
                return estateEntities.Buildings.FirstOrDefault(b=>b.BuildingID == BuildingID).BuildingName;
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

        #region Constructor
        public UpdateApartmentModalViewModel(Apartments apartment)
            : base()
        {
            item = apartment;
            Console.WriteLine(BuildingName);
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            if (BuildingID <= 0)
            {
                errors.Add("Id Budynku nie może być mniejszy lub równy 0");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(ApartmentNumber))
            {
                errors.Add("Numer Mieszkania jest wymagany.");
                isDataCorrect = false;
            }

            if (Area < 25)
            {
                errors.Add("Zgodnie z prawem Polskim powieszchnia mieszkania nie może mieć mniejsza niż 25m2");
                isDataCorrect = false;
            }

            if (RoomCount < 1)
            {
                errors.Add("Liczba pokoi nie może być mniejsza od 1");
                isDataCorrect = false;
            }

            if (Floor < 0)
            {
                errors.Add("Piętro nie może być liczbą ujemną.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status mieszkania jest wymagany.");
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
                var existingItem = estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID);
                if (existingItem != null)
                {
                    existingItem.ApartmentNumber = item.ApartmentNumber;
                    existingItem.Area = item.Area;
                    existingItem.RoomCount = item.RoomCount;
                    existingItem.Floor = item.Floor;
                    existingItem.Status = item.Status;
                    existingItem.BuildingID = item.BuildingID;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono elementu do aktualizacji.");
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
        // Zamiast wyświetlać alert, otwórz modal.
        var updateApartmentModal = new UpdateApartmentModal(); // To powinien być Twój UserControl/Window
        updateApartmentModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateApartmentModal.Close();
            };

            updateApartmentModal.ShowDialog(); // Jeśli to jest Window
    }

        #endregion
    }
}

