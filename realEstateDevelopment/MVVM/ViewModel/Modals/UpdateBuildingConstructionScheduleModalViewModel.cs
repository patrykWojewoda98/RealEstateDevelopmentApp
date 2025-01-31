using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;
using System;
using realEstateDevelopment.Helper;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Windows.Data;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateBuildingConstructionScheduleModalViewModel : BaseDataUpdater<ConstructionSchedule>
    {
        private Buildings _building;

        public int ScheduleId
        {
            get => item.ScheduleID;
            set
            {
                item.ScheduleID = value;
                OnPropertyChanged(() => ScheduleId);
            }
        }

        private ProjectEntityForView _project;
        public ProjectEntityForView Project
        {
            get => _project;
            set
            {
                _project = value;
                if (_project != null)
                {
                    item.ProjectID = _project.ProjectId;
                    _building = estateEntities.Buildings.FirstOrDefault(b => b.ProjectID == _project.ProjectId);
                }
                OnPropertyChanged(() => Project);
            }
        }

        public DateTime StartDate
        {
            get => item.StartDate;
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime? EndDate
        {
            get => item.EndDate;
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
            }
        }

        public string BuildingName => _building?.BuildingName ?? "Brak danych";
        public string BuildingNumber => _building?.BuildingNumber ?? "Brak danych";
        public int Floors => _building?.Floors ?? 0;
        public int NumberOfApartments => _building?.Apartments.Count ?? 0;

        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        public UpdateBuildingConstructionScheduleModalViewModel(ConstructionSchedule constructionSchedule)
            : base()
        {
            item = constructionSchedule;
            var project = estateEntities.Projects.FirstOrDefault(p => p.ProjectID == constructionSchedule.ProjectID);
            if (project != null)
            {
                Project = new ProjectEntityForView
                {
                    ProjectId = project.ProjectID,
                    ProjectName = project.ProjectName,
                    ProjectLocalization = project.Location
                };
            }
            _building = estateEntities.Buildings.FirstOrDefault(b => b.ProjectID == constructionSchedule.ProjectID);

            Console.WriteLine(Project);
        }

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (ScheduleId < 0)
            {
                errors.Add("Id nie może być mniejszy od 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status jest wymagany.");
                isDataCorrect = false;
            }

            if (StartDate == default)
            {
                errors.Add("Data rozpoczęcia jest wymagana.");
                isDataCorrect = false;
            }

            if (EndDate.HasValue && EndDate < StartDate)
            {
                errors.Add("Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }

        public override void Update()
        {
            Validate();
            if (isDataCorrect)
            {
                var existingItem = estateEntities.ConstructionSchedule.FirstOrDefault(c => c.ScheduleID == ScheduleId);
                if (existingItem != null)
                {
                    existingItem.Status = item.Status;
                    existingItem.StartDate = item.StartDate;
                    existingItem.EndDate = item.EndDate;

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
            var constructionScheduleModal = new UpdateBuildingConstructionScheduleModalView();
            constructionScheduleModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                constructionScheduleModal.Close();
            };

            constructionScheduleModal.ShowDialog();
        }
    }
}