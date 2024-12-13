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
    public class AddNewRevenueViewModel : BaseDatabaseAdder<Revenues>
    {
        private ProjectEntityForView _selectedProject;

        public ObservableCollection<ProjectEntityForView> AvailableProject { get; set; }

        public string _selectedType;
        public ObservableCollection<string> AvaibleTypes { get; set; }

        public ProjectEntityForView SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                if (_selectedProject != null)
                {
                    item.ProjectID = _selectedProject.ProjectId;
                }
                OnPropertyChanged(() => SelectedProject);
            }
        }

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                if (_selectedType != null)
                {
                    item.RevenueType = _selectedType;
                }
                OnPropertyChanged(() => SelectedType);
            }
        }

        public decimal Amount
        {
            get => item.Amount;
            set
            {
                item.Amount = value;
                OnPropertyChanged(() => Amount);
            }
        }

        public DateTime RevenueeDate
        {
            get => item.RevenueDate;
            set
            {
                item.RevenueDate = value;
                OnPropertyChanged(() => RevenueeDate);
            }
        }



        #region Constructor

        public AddNewRevenueViewModel()
        {
            item = new Revenues();

            AvailableProject = new ObservableCollection<ProjectEntityForView>();
            AvaibleTypes = new ObservableCollection<string>();
            LoadProjects();
            LoadTypes();

            SelectedProject = AvailableProject.FirstOrDefault();
            SelectedType = AvaibleTypes.FirstOrDefault();
            RevenueeDate = DateTime.Now;

        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (SelectedProject == null)
            {
                errors.Add("Nie wybrano Projektu.");
                isDataCorrect = false;
            }

            if (SelectedType == null)
            {
                errors.Add("Nie wybrano typu przychodu.");
                isDataCorrect = false;
            }
            if (Amount <= 0)
            {
                errors.Add("Kwota nie może być mniejsza bądz równa 0.");
                isDataCorrect = false;
            }
            if(RevenueeDate < DateTime.Now.AddYears(-1))
            {
                errors.Add("Data przychodzu nie może być starsza aniżeli rok");
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
                estateEntities.Revenues.Add(item);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }

        private void LoadProjects()
        {
            var projects = estateEntities.Projects.Select(c => new ProjectEntityForView
            {
                ProjectId = c.ProjectID,
                ProjectName = c.ProjectName,
                ProjectLocalization = c.Location
            }).ToList();

            foreach (var project in projects)
            {
                AvailableProject.Add(project);
            }
        }




        private void LoadTypes()
        {
            string[] types = { "Czynsz", "Sprzedarz", "Ubezpieczenia", "Naprawy", "Usługi dodatkowe" };

            foreach (var type in types)
            {
                AvaibleTypes.Add(type);
            }


            SelectedType = AvaibleTypes.FirstOrDefault();
        }
        #endregion
    }
}