using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class SalesViewModel : LoadAllViewModel<SalesEntityForView>
    {
        #region Helpers
        public override async Task LoadAsync()
        {

            var sales = realEstateEntities.Sales;
            var buildings = realEstateEntities.Buildings;
            var apartments = realEstateEntities.Apartments;
            var projects = realEstateEntities.Projects;
            var clients = realEstateEntities.Clients;


            var query = from s in sales
                        join c in clients on s.ClientID equals c.ClientID
                        join a in apartments on s.ApartmentID equals a.ApartmentID
                        join b in buildings on a.BuildingID equals b.BuildingID
                        join p in projects on b.ProjectID equals p.ProjectID
                        select new SalesEntityForView
                        {
                            Id = s.SaleID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            SaleDate = s.SaleDate,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            SalePrice = s.SalePrice,
                            Status = s.Status,
                            
                        };

            List = new ObservableCollection<SalesEntityForView>(query.ToList());


        }
        #endregion
    }
}
