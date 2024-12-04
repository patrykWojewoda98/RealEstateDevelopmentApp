﻿using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.Generic;
using System;
using realEstateDevelopment.MVVM.View.Modals;
using System.Linq;


namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewBuildingViewModel : BaseDatabaseAdder<Buildings>
    {
        #region Propeties
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

        public int ProjectID
        {
            get
            {
                return item.ProjectID;
            }
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
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

        
        public int Floors
        {
            get
            {
                return item.Floors;
            }
            set
            {
                item.Floors = value;
                OnPropertyChanged(() => Floors);
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
        public AddNewBuildingViewModel()
            : base()
        {
            item = new Buildings();
        }


        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            var db = new RealEstateEntities();
            
            if(db.Buildings.Any(b => b.BuildingNumber == item.BuildingNumber))
            {
                errors.Add("Budynek o podanym Numerze już istnieje.");
                isDataCorrect=false;
            }


            if (ProjectID <= 0)
            {
                errors.Add("Numer projektu nie może być mniejszy lub równy 0");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(BuildingName))
            {
                errors.Add("Nazwa budynku jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(BuildingNumber))
            {
                errors.Add("Numer budynku jest wymagany.");
                isDataCorrect = false;
            }

            if (Floors < 0)
            {
                errors.Add("Liczba pięter musi być większa bądź równa 0.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status budynku jest wymagany.");
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
                estateEntities.Buildings.Add(item);
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
