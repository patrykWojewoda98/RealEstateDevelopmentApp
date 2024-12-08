using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class SalesEntityForView
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; }
    }
}
