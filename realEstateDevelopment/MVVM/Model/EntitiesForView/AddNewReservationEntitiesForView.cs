namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class AddNewReservationEntitiesForView
    {
        public int ApartmentId {  get; set; }
        public string ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string Address {  get; set; }


        public int ClientId {  get; set; }
        public string ClientName { get; set; }
        public string ClientLastname {  get; set; }
        public string ClientPesel { get; set; }
    }
}
