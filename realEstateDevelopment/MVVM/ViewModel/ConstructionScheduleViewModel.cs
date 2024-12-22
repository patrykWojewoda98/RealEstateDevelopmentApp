using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ConstructionScheduleViewModel : LoadAllViewModel<ConstructionScheduleEntityForView>
    {
        #region Commands
        public RealyCommand OpenScheduleNewConstructionCommand { get; set; }
        #endregion
        public event Action ScheduleNewConstructionRequested;



        #region Constructor
        public ConstructionScheduleViewModel()
            :base()
        {

            OpenScheduleNewConstructionCommand = new RealyCommand(o =>
            {
                ScheduleNewConstructionRequested?.Invoke();
            });
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
                var query = from c in realEstateEntities.ConstructionSchedule
                       join b in realEstateEntities.Buildings on c.BuildingId equals b.BuildingID
                       join p in realEstateEntities.Projects on c.ProjectID equals p.ProjectID
                       where c.ProjectID==p.ProjectID 
                       select new ConstructionScheduleEntityForView
                       {
                           ScheduleID = c.ScheduleID,
                           BuildingId = b.BuildingID,
                           BuildingName = b.BuildingName,
                           BuildingNumber = b.BuildingNumber,
                           Floors = b.Floors,
                           Address = p.Location,
                           NumberOfApartments = realEstateEntities.Apartments.Count(a => a.BuildingID == b.BuildingID),
                           StartDate = c.StartDate,
                           EndDate = c.EndDate,
                           Status = c.Status,
                       };

            


            var result = await query.ToListAsync();
            List = new ObservableCollection<ConstructionScheduleEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
