using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MaintenanceRequestsViewModel : LoadAllViewModel<MaintenanceRequestsEntityForView>
    {
        #region Commands
        public RealyCommand OpenAddNewMaintenanceRequestsCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewMaintenanceRequestsRequested;
        #endregion

        #region
        public MaintenanceRequestsViewModel()
            :base()
        {
            OpenAddNewMaintenanceRequestsCommand = new RealyCommand(o =>
            {
                AddNewMaintenanceRequestsRequested?.Invoke();
            });
        }
        #endregion
        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from m in realEstateEntities.MaintenanceRequests
                        join c in realEstateEntities.Clients on m.ClientID equals c.ClientID
                        join a in realEstateEntities.Apartments on m.ApartmentID equals a.ApartmentID
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new MaintenanceRequestsEntityForView
                        {
                            Id = m.RequestID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            Description = m.Description,
                            RequestDate = m.RequestDate ?? default,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            ClientPhoneNumber = c.PhoneNumber,
                            Status = m.Status
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<MaintenanceRequestsEntityForView>(result);
        }
        #endregion
    }
}
