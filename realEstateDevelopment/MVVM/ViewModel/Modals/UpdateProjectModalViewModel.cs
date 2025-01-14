using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateProjectModalViewModel : BaseDataUpdater<Projects>
    {
        #region Properties

        public int ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
            }
        }

        public string ProjectName
        {
            get => item.ProjectName;
            set
            {
                item.ProjectName = value;
                OnPropertyChanged(() => ProjectName);
            }
        }

        public string Location
        {
            get => item.Location;
            set
            {
                item.Location = value;
                OnPropertyChanged(() => Location);
            }
        }

        public DateTime StartDate
        {
            get => (DateTime)item.StartDate;
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get {
                if (item.EndDate != null) {
                    return (DateTime)item.EndDate;
                }
                else return DateTime.MinValue;
            }
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
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

        public UpdateProjectModalViewModel(Projects project)
            : base()
        {
            item = project;
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (ProjectID <= 0)
            {
                errors.Add("Id projektu nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

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

            if (StartDate == default)
            {
                errors.Add("Data rozpoczęcia projektu jest wymagana.");
                isDataCorrect = false;
            }

            if (EndDate == default || EndDate < StartDate)
            {
                errors.Add("Data zakończenia projektu jest wymagana i nie może być wcześniejsza niż data rozpoczęcia.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status projektu jest wymagany.");
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
                var existingItem = estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID);
                if (existingItem != null)
                {
                    existingItem.ProjectName = item.ProjectName;
                    existingItem.Location = item.Location;
                    existingItem.StartDate = item.StartDate;
                    existingItem.EndDate = item.EndDate;
                    existingItem.Status = item.Status;

                    estateEntities.SaveChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono projektu do aktualizacji.");
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
            var updateProjectModal = new UpdateProjectModalView();
            updateProjectModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateProjectModal.Close();
            };

            updateProjectModal.ShowDialog();
        }

        #endregion
    }
}
