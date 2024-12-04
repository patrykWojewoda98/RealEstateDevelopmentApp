using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
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

        public override async Task LoadAsync()
        {
            var constructionSchedules = realEstateEntities.ConstructionSchedule;
            var buildings = realEstateEntities.Buildings;
            var apartments = realEstateEntities.Apartments;
            var project = realEstateEntities.Projects;


            List = new ObservableCollection<ConstructionScheduleEntityForView>(constructionSchedules.Join(buildings,
                                                                                            c => c.ProjectID,
                                                                                            b => b.ProjectID,
                                                                                            (c, b) => new ConstructionScheduleEntityForView
                                                                                            {
                                                                                                ScheduleID = c.ScheduleID,
                                                                                                BuildingName = b.BuildingName,
                                                                                                BuildingNumber = b.BuildingNumber,
                                                                                                Floors = b.Floors,
                                                                                                NumberOfApartments = apartments.Count(a => a.BuildingID == b.BuildingID),
                                                                                                StartDate = c.StartDate,
                                                                                                EndDate = c.EndDate,
                                                                                                Address = project.Where(p => p.ProjectID == c.ProjectID).Select(p => p.Location).FirstOrDefault()??"Adresu nie znaleziono",
                                                                                                Status = c.Status
                                                                                            })
                .ToList());


            
        }
        #endregion
    }
}
