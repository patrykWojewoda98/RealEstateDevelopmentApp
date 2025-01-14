using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ReservationsEntityForView
    {
        public int Id { get; set; }
        public string ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string Address { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientPhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{ClientName} {ClientSurname}";
        }
    }
}
