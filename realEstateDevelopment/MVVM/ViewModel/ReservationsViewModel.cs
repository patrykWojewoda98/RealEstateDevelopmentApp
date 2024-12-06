using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ReservationsViewModel :LoadAllViewModel<ReservationsEntityForView>
    {
        #region Constructor
        public ReservationsViewModel()
            :base()
        {
            
        }
        #endregion


        #region Helpers
        public override async Task LoadAsync()
        {
            var reservations = realEstateEntities.Reservations;
            var buildings = realEstateEntities.Buildings;
            var apartments = realEstateEntities.Apartments;
            var projects = realEstateEntities.Projects;
            var clients = realEstateEntities.Clients;


            var query = from r in reservations
                        join c in clients on r.ClientID equals c.ClientID
                        join a in apartments on r.ApartmentID equals a.ApartmentID
                        join b in buildings on a.BuildingID equals b.BuildingID
                        join p in projects on b.ProjectID equals p.ProjectID
                        select new ReservationsEntityForView
                        {
                            Id = r.ReservationID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            ReservationDate = r.ReservationDate,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            ClientPhoneNumber = c.PhoneNumber,
                        };

            List = new ObservableCollection<ReservationsEntityForView>(query.ToList());



        }
        #endregion
    }
}
