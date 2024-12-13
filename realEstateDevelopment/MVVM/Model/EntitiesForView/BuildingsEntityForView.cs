namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class BuildingsEntityForView
    {
        #region Properties
        public int BuildingID { get; set; }

        public string BuildingNumber { get; set; }

        public int ProjectID { get; set; }
        public string BuildingName { get; set; }
        public int Floors { get; set; }
        public string Status { get; set; }

        public string Localization { get; set; }

        public override string ToString()
        {
            return $"Budynek numer: {BuildingNumber} w {Localization}";
        }
        #endregion
    }
}
