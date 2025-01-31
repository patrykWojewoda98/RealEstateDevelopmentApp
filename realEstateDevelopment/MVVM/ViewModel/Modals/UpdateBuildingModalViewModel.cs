using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateBuildingModalViewModel:BaseDataUpdater<Buildings>
    {
        #region Propeties

        private BuildingsEntityForView _selectedBuilding;


        public BuildingsEntityForView SelectedBuilding
        {
            get => _selectedBuilding;

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
                return item.BuildingName;
            }
            set
            {
                item.BuildingName = value;
                OnPropertyChanged(() => BuildingName);
            }
        }
        public string BuildingNumber
        {
            get
            {
                return item.BuildingNumber;
            }
            set
            {
                item.BuildingNumber = value;
                OnPropertyChanged(() => BuildingNumber);
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
        public UpdateBuildingModalViewModel(Buildings building)
            : base()
        {
            item = building;
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

            if (string.IsNullOrWhiteSpace(BuildingName))
            {
                errors.Add("Nazwa Budynku jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(BuildingNumber))
            {
                errors.Add("Numer Budynku jest wymagany.");
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
                var existingItem = estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == item.BuildingID);
                if (existingItem != null)
                {
                    existingItem.BuildingName = item.BuildingName;
                    existingItem.BuildingNumber = item.BuildingNumber;
                    existingItem.Status = item.Status;

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
            var updateBuildingModal = new UpdateBuildingModalView(); // To powinien być Twój UserControl/Window
            updateBuildingModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateBuildingModal.Close();
            };

            updateBuildingModal.ShowDialog(); // Jeśli to jest Window
        }

        #endregion
    }
}