namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ClientForView
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Pesel { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int IdCardNumber { get; set; }
        public string IdCardSeries { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} (Pesel: {Pesel})";
        }
    }
}
