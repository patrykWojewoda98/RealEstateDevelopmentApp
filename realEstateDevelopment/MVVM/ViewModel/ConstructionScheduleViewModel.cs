using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ConstructionScheduleViewModel:LoadAllViewModel<ConstructionScheduleEntityForView>
    {
        


        #region Constructor
        public ConstructionScheduleViewModel()
            :base()
        {
            
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from c in realEstateEntities.ConstructionSchedule
                        join b in realEstateEntities.Buildings on c.ProjectID equals b.ProjectID
                        join p in realEstateEntities.Projects on c.ProjectID equals p.ProjectID
                        select new ConstructionScheduleEntityForView
                        {
                            ScheduleID = c.ScheduleID,
                            BuildingName = b.BuildingName,
                            BuildingNumber = b.BuildingNumber,
                            Floors = b.Floors,
                            NumberOfApartments = realEstateEntities.Apartments.Count(a => a.BuildingID == b.BuildingID),
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            Address = p.Location ,
                            Status = c.Status
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ConstructionScheduleEntityForView>(result);
        }
        #endregion
    }
}
