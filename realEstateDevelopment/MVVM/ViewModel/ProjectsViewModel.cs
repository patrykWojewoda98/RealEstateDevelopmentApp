using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows.Input;

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

        private string _projectNameFilter;
        public string ProjectNameFilter
        {
            get => _projectNameFilter;
            set
            {
                _projectNameFilter = value;
                OnPropertyChanged(() => ProjectNameFilter);
                ApplyFiltersAsync();
            }
        }

        private string _locationFilter;
        public string LocationFilter
        {
            get => _locationFilter;
            set
            {
                _locationFilter = value;
                OnPropertyChanged(() => LocationFilter);
                ApplyFiltersAsync();
            }
        }

        private DateTime _startDateFromFilter;
        public DateTime StartDateFromFilter
        {
            get => _startDateFromFilter;
            set
            {
                _startDateFromFilter = value;
                OnPropertyChanged(() => StartDateFromFilter);
                ApplyFiltersAsync();
            }
        }

        private DateTime _startDateToFilter;
        public DateTime StartDateToFilter
        {
            get => _startDateToFilter;
            set
            {
                _startDateToFilter = value;
                OnPropertyChanged(() => StartDateToFilter);
                ApplyFiltersAsync();
            }
        }

        private string _statusFilter;
        public string StatusFilter
        {
            get => _statusFilter;
            set
            {
                _statusFilter = value;
                OnPropertyChanged(() => StatusFilter);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<ProjectEntityForView> _filteredList;
        public ObservableCollection<ProjectEntityForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        #endregion

        #region Commands
        public RealyCommand ApplyFiltersCommand { get; set; }
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
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
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
            FilteredList = new ObservableCollection<ProjectEntityForView>(
                            List.Where(p =>
                                (string.IsNullOrEmpty(ProjectNameFilter) || p.ProjectName.Contains(ProjectNameFilter)) &&
                                (string.IsNullOrEmpty(LocationFilter) || p.ProjectLocalization.Contains(LocationFilter)) &&
                                (StartDateFromFilter != null || (p.StartDate != null && p.StartDate >= StartDateFromFilter)) &&
                                (StartDateToFilter != null || (p.StartDate != null && p.StartDate <= StartDateToFilter)) &&
                                (string.IsNullOrEmpty(StatusFilter) || p.Status.Contains(StatusFilter))
                            ));


            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
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