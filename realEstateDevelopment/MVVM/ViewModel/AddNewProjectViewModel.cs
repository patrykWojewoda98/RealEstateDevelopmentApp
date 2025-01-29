using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewProjectViewModel : BaseDatabaseAdder<Projects>
    {
        #region Propeties
        public string ProjectName
        {
            get
            {
                return item.ProjectName;
            }
            set
            {
                item.ProjectName = value;
                OnPropertyChanged(() => ProjectName);
            }
        }

        public string Location
        {
            get
            {
                return item.Location;
            }
            set
            {
                item.Location = value;
                OnPropertyChanged(() => Location);
            }
        }
    
        public DateTime StartDate
        {
            get
            {
                return item?.StartDate?.Date ?? DateTime.MinValue;
            }
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return item?.EndDate?.Date ?? DateTime.MinValue;
            }
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
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
        public AddNewProjectViewModel()
            : base()
        {
            item = new Projects();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }


        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();


            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                errors.Add("Nazwa projektu jest wymagana.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(Location))
            {
                errors.Add("Lokalizacja projektu jest wymagana.");
                isDataCorrect = false;
            }
            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ProjectName):
                    return string.IsNullOrWhiteSpace(ProjectName) ? "Nazwa projektu jest wymagana." : string.Empty;

                case nameof(Location):
                    return string.IsNullOrWhiteSpace(Location) ? "Lokalizacja projektu jest wymagana." : string.Empty;

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
                estateEntities.Projects.Add(item);
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