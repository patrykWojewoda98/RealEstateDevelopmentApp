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
    public class AddNewExpenseViewModel:BaseDatabaseAdder<Expenses>
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
                    item.ExpenseType = _selectedType;
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

        public DateTime ExpenseDate
        {
            get => item.ExpenseDate;
            set
            {
                item.ExpenseDate = value;
                OnPropertyChanged(() => ExpenseDate);
            }
        }



        #region Constructor

        public AddNewExpenseViewModel()
        {
            item = new Expenses();

            AvailableProject = new ObservableCollection<ProjectEntityForView>();
            AvaibleTypes = new ObservableCollection<string>();
            LoadProjects();
            LoadTypes();

            SelectedProject = AvailableProject.FirstOrDefault();
            SelectedType = AvaibleTypes.FirstOrDefault();
            ExpenseDate = DateTime.Now;

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
            if (ExpenseDate < DateTime.Now.AddYears(-1))
            {
                errors.Add("Data kosztu nie może być starsza aniżeli rok");
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
                estateEntities.Expenses.Add(item);
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
            string[] types = { "Podatek", "Koszty Administracyjne", "Ubezpieczenia", "Naprawy", "Usługi dodatkowe", "Zakup gruntu", "Zakup Materiałów" };

            foreach (var type in types)
            {
                AvaibleTypes.Add(type);
            }


            SelectedType = AvaibleTypes.FirstOrDefault();
        }
        #endregion
    }
}