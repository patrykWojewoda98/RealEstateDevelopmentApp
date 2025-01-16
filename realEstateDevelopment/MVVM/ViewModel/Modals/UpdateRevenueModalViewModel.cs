using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateRevenueModalViewModel : BaseDataUpdater<Revenues>
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

        public int RevenueId
        {
            get => item.RevenueID;
            set
            {
                item.RevenueID = value;
                OnPropertyChanged(() => RevenueId);
            }
        }

        public string RevenueType
        {
            get => item.RevenueType;
            set
            {
                item.RevenueType = value;
                OnPropertyChanged(() => RevenueType);
            }
        }

        public string ProjectName
        {
            get => estateEntities.Projects.FirstOrDefault(p=>p.ProjectID == item.ProjectID).ProjectName;
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

        public DateTime RevenueDate
        {
            get => item.RevenueDate;
            set
            {
                item.RevenueDate = value;
                OnPropertyChanged(() => RevenueDate);
            }
        }

        #endregion

        #region Constructor

        public UpdateRevenueModalViewModel(Revenues revenue)
            : base()
        {
            item = revenue;
            AvailableProjects = new ObservableCollection<ProjectEntityForView>();
            LoadProjects();
            SelectedProject = AvailableProjects.FirstOrDefault(p=>p.ProjectId==item.ProjectID);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (RevenueId <= 0)
            {
                errors.Add("ID przychodu nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(RevenueType))
            {
                errors.Add("Typ przychodu jest wymagany.");
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

            if (RevenueDate == default(DateTime))
            {
                errors.Add("Data przychodu jest wymagana.");
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
                var existingItem = estateEntities.Revenues.FirstOrDefault(r => r.RevenueID == RevenueId);
                if (existingItem != null)
                {
                    existingItem.RevenueType = item.RevenueType;
                    existingItem.Amount = item.Amount;
                    existingItem.RevenueDate = item.RevenueDate;
                    existingItem.ProjectID = item.ProjectID;

                    estateEntities.SaveChanges();
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
            var updateRevenueModal = new UpdateRevenueModalView();
            updateRevenueModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateRevenueModal.Close();
            };

            updateRevenueModal.ShowDialog();
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
