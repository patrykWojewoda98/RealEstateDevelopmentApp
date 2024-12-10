using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class BuildingsViewModel : LoadAllViewModel<BuildingsEntityForView>
    {
        #region Commands
        public RealyCommand OpenAddNewBuildingCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewBuildingRequested;
        #endregion


        #region Constructor
        public BuildingsViewModel()
            : base()
        {
            OpenAddNewBuildingCommand = new RealyCommand(o =>
            {
                AddNewBuildingRequested?.Invoke();
            });
        }
        #endregion


        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from b in realEstateEntities.Buildings
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new BuildingsEntityForView
                        {
                            BuildingID = b.BuildingID,
                            BuildingName = b.BuildingName,
                            ProjectID = b.ProjectID,
                            BuildingNumber = b.BuildingNumber,
                            Floors = b.Floors,
                            Status = b.Status,
                        };
            var result = await query.ToListAsync();
            List = new ObservableCollection<BuildingsEntityForView>(result);
        }
        #endregion
    }
}