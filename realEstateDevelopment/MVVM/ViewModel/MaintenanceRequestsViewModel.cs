using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MaintenanceRequestsViewModel : LoadAllViewModel<MaintenanceRequestsEntityForView>
    {
        #region Helpers
        public override async Task LoadAsync()
        {
            var maintenanceRequests = realEstateEntities.MaintenanceRequests;
            var clients = realEstateEntities.Clients;
            var buildings = realEstateEntities.Buildings;
            var project = realEstateEntities.Projects;
            var apartments = realEstateEntities.Apartments;

            var query = from m in maintenanceRequests
                        join c in clients on m.ClientID equals c.ClientID
                        join a in apartments on m.ApartmentID equals a.ApartmentID
                        join b in buildings on a.BuildingID equals b.BuildingID
                        join p in project on b.ProjectID equals p.ProjectID
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


            List = new ObservableCollection<MaintenanceRequestsEntityForView>(query.ToList());
            
                



        }
        #endregion
    }
}
