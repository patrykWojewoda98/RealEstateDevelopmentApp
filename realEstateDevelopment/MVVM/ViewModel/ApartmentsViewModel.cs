using realEstateDevelopment.Core;

using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
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
            var apartments = realEstateEntities.Apartments;
            var buildings = realEstateEntities.Buildings;
            var project = realEstateEntities.Projects;

            List = new ObservableCollection<ApartmentsEntitiesForView>(apartments.Join(buildings,
                                                                                            a => a.BuildingID,
                                                                                            b => b.BuildingID,
                                                                                            (a, b) => new ApartmentsEntitiesForView
                                                                                            {
                                                                                                ApartmentID = a.ApartmentID,
                                                                                                BuildingID = b.BuildingID,
                                                                                                Address = buildings.Join(project,
                                                                                                                         bu => bu.ProjectID,
                                                                                                                         p => p.ProjectID,
                                                                                                                         (bu, p) => new { bu, p })
                                                                                                                         .Select(bp => bp.p.Location)
                                                                                                                         .FirstOrDefault(),
                                                                                                ApartmentNumber = a.ApartmentNumber,
                                                                                                Floor = a.Floor,
                                                                                                Area = a.Area,
                                                                                                Status = a.Status,
                                                                                                RoomCount = a.RoomCount
                                                                                            })
                .ToList());



        }
        #endregion

    }
}