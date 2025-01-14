using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ProjectsViewModel: LoadAllViewModel<ProjectEntityForView>
    {
    #region Commands
        public RealyCommand OpenAddNewProjectCommand { get; set; }
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


        #endregion
    }
}