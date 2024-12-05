using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class MaintenanceRequestsEntityForView
    {
        public int Id { get; set; }
        public string ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string Address {  get; set; }
        public string Description {  get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string Status {  get; set; }

    }
}
