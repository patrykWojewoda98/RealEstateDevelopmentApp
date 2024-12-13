using realEstateDevelopment.Core;

using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ApartmentsViewModel : LoadAllViewModel<ApartmentsEntitiesForView>
    {
        #region Commands
        public RealyCommand OpenAddNewApartmentCommand { get; set; }
        #endregion
        public event Action AddNewApartmentRequested;
        #region
        public ApartmentsViewModel()
            : base()
        {
            OpenAddNewApartmentCommand = new RealyCommand(o =>
            {
                AddNewApartmentRequested?.Invoke();
            });
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from a in realEstateEntities.Apartments
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new ApartmentsEntitiesForView
                        {
                            ApartmentID = a.ApartmentID,
                            BuildingID = b.BuildingID,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            ApartmentNumber = a.ApartmentNumber,
                            Floor = a.Floor,
                            Area = a.Area,
                            Status = a.Status,
                            RoomCount = a.RoomCount
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ApartmentsEntitiesForView>(result);



        }
        #endregion

    }
}