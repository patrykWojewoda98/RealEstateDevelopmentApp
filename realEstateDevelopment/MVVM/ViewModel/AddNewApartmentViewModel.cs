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
    internal class AddNewApartmentViewModel : BaseDatabaseAdder<Apartments>
    {

        #region Propeties

        private BuildingsEntityForView _selectedBuilding;

        public ObservableCollection<BuildingsEntityForView> AvailableBuilding { get; set; }


        public BuildingsEntityForView SelectedBuilding
        {
            get => _selectedBuilding;
            set
            {
                _selectedBuilding = value;
                if (_selectedBuilding != null)
                {
                    item.BuildingID = _selectedBuilding.BuildingID;
                }
                OnPropertyChanged(() => SelectedBuilding);
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
        public AddNewApartmentViewModel()
            : base()
        {
            item = new Apartments();

            AvailableBuilding = new ObservableCollection<BuildingsEntityForView>();
            LoadBuildings();

            SelectedBuilding = AvailableBuilding.FirstOrDefault();
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

        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(BuildingID):
                    return BuildingID <= 0 ? "Id Budynku nie może być mniejszy lub równy 0" : string.Empty;

                case nameof(ApartmentNumber):
                    return string.IsNullOrWhiteSpace(ApartmentNumber) ? "Numer mieszkania jest wymagany." : string.Empty;

                case nameof(Area):
                    return Area < 25 ? "Zgodnie z prawem Polskim powieszchnia mieszkania nie może być mniejsza niż 25m2" : string.Empty;

                case nameof(RoomCount):
                    return RoomCount < 1 ? "Liczba pokoi nie może być mniejsza od 1" : string.Empty;

                case nameof(Floor):
                    return Floor < 0 ? "Piętro nie może być liczbą ujemną." : string.Empty;

                case nameof(Status):
                    return string.IsNullOrWhiteSpace(Status) ? "Status mieszkania jest wymagany." : string.Empty;

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
                estateEntities.Apartments.Add(item);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }
        private void LoadBuildings()
        {
            var projects = estateEntities.Projects;
            var buildings = estateEntities.Buildings;

            var allbuildings = buildings.Join(projects,
                                              b => b.ProjectID,
                                              p => p.ProjectID,
                                              (b, p) => new BuildingsEntityForView
                                              {
                                                  BuildingID = b.BuildingID,
                                                  BuildingNumber = b.BuildingNumber,
                                                  BuildingName = b.BuildingName,
                                                  Localization = p.Location
                                              }).ToList();

            foreach (var building in allbuildings)
            {
                AvailableBuilding.Add(building);
            }
        }
        #endregion
    }
}
