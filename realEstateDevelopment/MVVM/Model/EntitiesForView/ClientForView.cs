namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ClientForView
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Pesel {  get; set; }
        public override string ToString()
        {
            return $"{Name} {Surname} (Pesel: {Pesel})";
        }
    }
}
