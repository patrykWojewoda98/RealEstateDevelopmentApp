namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ApartmentForView
    {
        public int Id { get; set; }
        public string ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"Mieszkanie: {ApartmentNumber}, Budynek: {BuildingNumber}, Adres: {Address}";
        }
    }
}