using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ProjectsViewModel: LoadAllViewModel<Projects>
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
        var projects = await Task.Run(() => realEstateEntities.Projects.ToList());
        List = new ObservableCollection<Projects>(projects);
    }


    #endregion
}
}