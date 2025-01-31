using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ProjectsViewModel: LoadAllViewModel<ProjectEntityForView>
    {
        #region Properties
        private ProjectEntityForView _selectedItem;
        public ProjectEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion

        #region Commands
        public RealyCommand OpenAddNewProjectCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewProjectRequested;
    #endregion


    #region Constructor
    public ProjectsViewModel()
        : base()
    {
            OpenAddNewProjectCommand = new RealyCommand(o =>
        {
            AddNewProjectRequested?.Invoke();
        });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }
    #endregion


    #region Helpers
    public override async Task LoadAsync()
    {
            var projects = from p in realEstateEntities.Projects
                           select new ProjectEntityForView
                           {
                               ProjectId = p.ProjectID,
                               ProjectLocalization = p.Location,
                               ProjectName = p.ProjectName,
                               StartDate = (DateTime)p.StartDate,
                               EndDate = (DateTime)p.EndDate,
                               Status = p.Status,

                           };

        List = new ObservableCollection<ProjectEntityForView>(projects);
    }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ProjectEntityForView selected)
            {
                var modal = new DeleteProjectModalView();
                DeleteProjectModalViewModel deleteProjectModalViewModel = new DeleteProjectModalViewModel(
                                            realEstateEntities.Projects.First(p => p.ProjectID == SelectedItem.ProjectId));
                deleteProjectModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteProjectModalViewModel;
                modal.Show();

            }
        }


        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null; // Polecenie dostępne tylko, jeśli coś jest zaznaczone.
        }

        #endregion
    }
}