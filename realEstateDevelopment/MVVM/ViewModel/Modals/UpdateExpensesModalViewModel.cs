using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateExpensesModalViewModel : BaseDataUpdater<Expenses>
    {
        #region Properties


        private ProjectEntityForView _selectedProject;


        public ObservableCollection<ProjectEntityForView> AvailableProjects { get; set; }

        public ProjectEntityForView SelectedProject
        {
            get
            {
                return _selectedProject;
            }

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

        public int ExpenseId
        {
            get => item.ExpenseID;
            set
            {
                item.ExpenseID = value;
                OnPropertyChanged(() => ExpenseId);
            }
        }

        public string ExpenseType
        {
            get => item.ExpenseType;
            set
            {
                item.ExpenseType = value;
                OnPropertyChanged(() => ExpenseType);
            }
        }

        public string ProjectName
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID).ProjectName;
            set
            {
                OnPropertyChanged(() => ProjectName);
            }
        }

        public string ProjectAddress
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID).Location;
            set
            {
                OnPropertyChanged(() => ProjectAddress);
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

        #endregion

        #region Constructor

        public UpdateExpensesModalViewModel(Expenses expense)
            : base()
        {
            item = expense;
            AvailableProjects = new ObservableCollection<ProjectEntityForView>();
            LoadProjects();
            SelectedProject = AvailableProjects.FirstOrDefault(p => p.ProjectId == item.ProjectID);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (ExpenseId <= 0)
            {
                errors.Add("ID kosztu nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(ExpenseType))
            {
                errors.Add("Typ kosztu jest wymagany.");
                isDataCorrect = false;
            }

            if (Amount <= 0)
            {
                errors.Add("Kwota przychodu musi być większa od 0.");
                isDataCorrect = false;
            }

            if (SelectedProject == null || SelectedProject.ProjectId <= 0)
            {
                errors.Add("Projekt jest wymagany.");
                isDataCorrect = false;
            }

            if (ExpenseDate == default(DateTime))
            {
                errors.Add("Data kosztu jest wymagana.");
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
                var existingItem = estateEntities.Expenses.FirstOrDefault(e => e.ExpenseID == ExpenseId);
                if (existingItem != null)
                {
                    existingItem.ExpenseType = item.ExpenseType;
                    existingItem.Amount = item.Amount;
                    existingItem.ExpenseDate = item.ExpenseDate;
                    existingItem.ProjectID = item.ProjectID;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono przychodu do aktualizacji.");
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
            var updateExpenseModal = new UpdateExpensesModalView();
            updateExpenseModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateExpenseModal.Close();
            };

            updateExpenseModal.ShowDialog();
        }

        private void LoadProjects()
        {
            var projects = (from p in estateEntities.Projects
                            select new ProjectEntityForView
                            {
                                ProjectId = p.ProjectID,
                                ProjectLocalization = p.Location,
                                ProjectName = p.ProjectName,
                                EndDate = (DateTime)p.EndDate,
                                StartDate = (DateTime)p.StartDate,
                                Status = p.Status,
                            }).ToList();

            foreach (var project in projects)
            {
                AvailableProjects.Add(project);
            }
        }

        #endregion
    }
}